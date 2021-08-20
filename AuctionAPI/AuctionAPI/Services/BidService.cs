using AuctionAPI.Interfaces.PublisherSubscriber;
using AuctionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuctionAPI.Data
{
    public class BidService : IBidService
    {
        private BidContext _bidContext;
        private AutoBidContext _autoBidContext;
        private readonly IPubSub _publishSubscribe;
        public BidService(BidContext bidContext, AutoBidContext autoBidContext, IPubSub publishSubscribe)
        {
            _autoBidContext = autoBidContext;
            _bidContext = bidContext;
            _publishSubscribe = publishSubscribe;
        }
        public List<Bid> GetBids(bool sortDescendingByTimeOfBidding = true)
        {
            return _bidContext.Bid.Where(b => b.TimeOfBidding < b.ItemExpiry).OrderByDescending(b => b.TimeOfBidding).ToList();
        }
        public List<Bid> GetBidsForItem(int itemId)
        {
            return GetBids().Where(b => b.ItemId == itemId).OrderByDescending(i => i.Offer).Take(5).ToList();
        }
        public Bid AddBid(Bid bid)
        {
            lock (_bidContext)
            {
                Bid lastBid = _getLastBid(bid.ItemId);
                if (bid.ItemExpiry < DateTime.Now || (lastBid != null && bid.Offer <= lastBid.Offer) || bid.Offer < 1 )
                {
                    return null;
                }
                
                bid.TimeOfBidding = DateTime.Now;
                _bidContext.Bid.Add(bid);
                _bidContext.SaveChanges();
                _handleAutoBids(bid.BidderId, bid.ItemId, bid.Offer, bid.ItemExpiry,lastBid);
                return _bidContext.Bid.Where(b => bid.Id == b.Id).FirstOrDefault();
            } 
        }
        public void CreateAutoBid(AutoBid autoBid, DateTime expiry)
        {
            lock (_bidContext)
            {
                lock (_autoBidContext)
                {
                    if (!_autoBidContext.AutoBid.Any(a => a.BidderId == autoBid.BidderId && a.ItemId == autoBid.ItemId))
                    {
                        _autoBidContext.AutoBid.Add(autoBid);
                    }
                    _autoBidContext.AutoBid.Where(a => a.BidderId == autoBid.BidderId).ToList().ForEach(a => a.Budget = autoBid.Budget);
                    _autoBidContext.SaveChanges();
                    Bid lastBid = _getLastBid(autoBid.ItemId);
                    if (lastBid != null)
                    {
                        if (lastBid.Offer + 1 <= autoBid.Budget && lastBid.ItemExpiry > DateTime.Now && lastBid.BidderId != autoBid.BidderId)
                        {
                            _autoBidMakeBid(autoBid, lastBid,true);
                        }
                    }
                    else
                    {
                        _autoBidInitialBid(autoBid, expiry);
                    }
                    _autoBidContext.AutoBid.Where(a => a.BidderId == autoBid.BidderId && a.ItemId != autoBid.ItemId).ToList().ForEach(a => _attemptAutoBid(a));
                }
            }
        }
        public bool RemoveAutoBid(AutoBid autoBid)
        {
            lock (_autoBidContext)
            {
                var a = _autoBidContext.AutoBid.Where(a => a.ItemId == autoBid.ItemId && a.BidderId == autoBid.BidderId).FirstOrDefault();
                _autoBidContext.Remove(a);
                _autoBidContext.SaveChanges();
                return _autoBidContext.AutoBid.Any(a => a.BidderId == autoBid.BidderId);
            }
            
        }
        public bool HasAutoBid(int bidderId,int itemId)
        {
            return _autoBidContext.AutoBid.Any(a => a.BidderId == bidderId && a.ItemId == itemId);
        }
        public int GetAutoBidBudget(int bidderId)
        {
            var autoBid = _autoBidContext.AutoBid.Where(a => a.BidderId == bidderId).FirstOrDefault();
            return autoBid == null ? 0 : autoBid.Budget;
        }
        private Bid _getLastBid(int itemId)
        {
            var itemBids = _bidContext.Bid.Where(b => b.ItemId == itemId).ToList();
            return itemBids.Count == 0 ? null : itemBids?.Aggregate((i, j) => i.Offer > j.Offer ? i : j);
        }
        private void _raiseEvent(int bidderId, int itemId, int offer, DateTime expiry)
        {
            _publishSubscribe.PublishEvent(this, bidderId, itemId, offer, expiry);
        }
        private void _handleAutoBids(int bidderId, int itemId, int offer, DateTime expiry, Bid lastBid, bool autoBidCreation = false)
        {
            lock (_autoBidContext)
            {
                if (!_autoBidContext.AutoBid.Any(a => a.ItemId == itemId))
                {
                    _raiseEvent(bidderId, itemId, offer, expiry);
                    return;
                }
                bool needToRetrievePreviousBidder = lastBid != null && _autoBidContext.AutoBid.Any(a => a.BidderId == lastBid.BidderId && a.ItemId == lastBid.ItemId) && lastBid.AutoBid;
                if (needToRetrievePreviousBidder) _autoBidContext.AutoBid.Where(a => a.BidderId == lastBid.BidderId).ToList().ForEach(a => a.Budget += lastBid.Offer);
                var itemBids = _autoBidContext.AutoBid.Where(a => a.ItemId == itemId).ToList();
                bool clear = true;
                int i = 0;
                int newOffer = offer + 1;
                int newBidder = bidderId;
                bool autoBidHappened = false;
                while (i < itemBids.Count)
                {
                    AutoBid autoBid = itemBids[i];
                    if (autoBid.BidderId == newBidder)
                    {
                        i++;
                        if (i == itemBids.Count)
                        {
                            if (clear) break;
                            clear = true;
                            i = 0;
                        }
                        continue;
                    }
                    if (autoBid.Budget >= newOffer)
                    {
                        Bid bid = new Bid(autoBid.BidderId, autoBid.ItemId, newOffer, DateTime.Now, expiry, true);
                        _bidContext.Bid.Add(bid);
                        newOffer++;
                        newBidder = autoBid.BidderId;
                        clear = false;
                        autoBidHappened = true;
                    }
                    i++;
                    if (i == itemBids.Count)
                    {
                        if (clear) break;
                        clear = true;
                        i = 0;
                    }
                }
                if (autoBidHappened || autoBidCreation) _autoBidContext.AutoBid.Where(a => a.BidderId == newBidder).ToList().ForEach(a => a.Budget -= (newOffer - 1));
                _raiseEvent(newBidder, itemId, newOffer - 1, expiry);
                _bidContext.SaveChanges();
                _autoBidContext.SaveChanges();
                if (needToRetrievePreviousBidder && newBidder != lastBid.BidderId) _autoBidContext.AutoBid.Where(a => a.BidderId == lastBid.BidderId).ToList().ForEach(a => _attemptAutoBid(a));
            }
        }
        private void _attemptAutoBid(AutoBid autoBid)
        {
            if ( autoBid.Budget < 1) return;
            Bid lastBid = _getLastBid(autoBid.ItemId);
            if(lastBid.BidderId != autoBid.BidderId && lastBid.ItemExpiry > DateTime.Now && lastBid.Offer+1 <= autoBid.Budget)
            {
                _autoBidMakeBid(autoBid, lastBid);
            }
        }
        private Bid _autoBidAddBidToBids(AutoBid autoBid,DateTime expiry,int offer)
        {
            Bid bid = new Bid(autoBid.BidderId, autoBid.ItemId, offer, DateTime.Now, expiry, true);
            _bidContext.Add(bid);
            _bidContext.SaveChanges();
            _autoBidContext.AutoBid.Where(a => a.BidderId == autoBid.BidderId).ToList().ForEach(a => a.Budget -= offer);
            return bid;
        }
        private void _autoBidInitialBid(AutoBid autoBid,DateTime expiry)
        {
            _autoBidAddBidToBids(autoBid, expiry, 1);
            _autoBidContext.SaveChanges();
            _raiseEvent(autoBid.BidderId, autoBid.ItemId, 1, expiry);
        }
        private void _autoBidMakeBid(AutoBid autoBid,Bid lastBid, bool creation = false)
        {
            Bid bid = _autoBidAddBidToBids(autoBid, lastBid.ItemExpiry, lastBid.Offer + 1);
            bool previousBidderWasAutobidding = _autoBidContext.AutoBid.Any(a => a.BidderId == lastBid.BidderId && a.ItemId == lastBid.ItemId) && lastBid.AutoBid;
            if (previousBidderWasAutobidding) _autoBidContext.AutoBid.Where(a => a.BidderId == lastBid.BidderId).ToList().ForEach(a => a.Budget += lastBid.Offer);
            _autoBidContext.SaveChanges();
            _handleAutoBids(autoBid.BidderId, autoBid.ItemId, lastBid.Offer + 1, lastBid.ItemExpiry, bid, creation);
            if(previousBidderWasAutobidding) _autoBidContext.AutoBid.Where(a => a.BidderId == lastBid.BidderId).ToList().ForEach(a => _attemptAutoBid(a));
        }
    }
}
