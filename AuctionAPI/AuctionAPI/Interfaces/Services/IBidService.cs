using AuctionAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionAPI.Data
{
    public interface IBidService
    {
        List<Bid> GetBids(bool sortByTimeOfBidding=true);
        List<Bid> GetBidsForItem(int itemId);
        Bid AddBid(Bid bid);
        void CreateAutoBid(AutoBid autoBid, DateTime expiry);
        bool RemoveAutoBid(AutoBid autoBid);
        bool HasAutoBid(int bidderId, int itemId);
        int GetAutoBidBudget(int bidderId);
    }
}
