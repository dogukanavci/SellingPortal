using AuctionAPI.Interfaces.PublisherSubscriber;
using AuctionAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace AuctionAPI.Data
{
    public class BidderService : IBidderService
    {
        private BidderContext _bidderContext;
        public BidderService(BidderContext bidderContext, IPubSub publishSubscribe)
        {
            _bidderContext = bidderContext;
        }
        public List<Bidder> GetBidders()
        {
            return _bidderContext.Bidder.ToList();
        }
        public Bidder GetBidder(int id)
        {
            return _bidderContext.Bidder.Where(br => br.Id == id).FirstOrDefault();
        }
    }
}
