using AuctionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Data
{
    public interface IBidderService
    {
        List<Bidder> GetBidders();
        Bidder GetBidder(int id);
    }
}
