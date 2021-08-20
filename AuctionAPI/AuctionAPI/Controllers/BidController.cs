using AuctionAPI.Data;
using AuctionAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private IBidService _bidService;
        private IBidderService _bidderService;
        private IItemService _itemService;


        public BidController(IBidService bidService, IBidderService bidderService, IItemService itemService)
        {
            _bidService = bidService;
            _bidderService = bidderService;
            _itemService = itemService;
        }

        [Route("GetBidsForItem/{itemId}")]
        public JsonResult GetBidsForItem(int itemId)
        {
            List<Bid> bids = _bidService.GetBidsForItem(itemId);
            Dictionary<int, string> bidderNameLookup = new Dictionary<int, string>();
            foreach (var bid in bids)
            {
                if (bidderNameLookup.ContainsKey(bid.BidderId)) continue;
                bidderNameLookup.Add(bid.BidderId, _bidderService.GetBidder(bid.BidderId).Name);
            }
            var result = new { Bids = bids, BidderNameLookup = bidderNameLookup };
            return new JsonResult(result);
        }

        [Route("SetAutobid/{bidderId}/{itemId}/{budget}/{autoBidMode}")]
        public JsonResult SetAutobid(int bidderId, int itemId, int budget, bool autoBidMode)
        {
            if (budget < 1 && autoBidMode) return null;
            AutoBid autoBid = new AutoBid();
            autoBid.BidderId = bidderId;
            autoBid.Budget = budget;
            autoBid.ItemId = itemId;
            DateTime expiry = _itemService.GetItem(itemId).Expiry;
            if (autoBidMode)
            {
                _bidService.CreateAutoBid(autoBid,expiry);
            }
            else
            {
                _bidService.RemoveAutoBid(autoBid);
            }

            return new JsonResult(new { Budget = budget, Mode = autoBidMode });
        }

        [Route("GetAutoBid/{bidderId}/{itemId}")]
        public JsonResult GetAutobid(int bidderId, int itemId)
        {
            var result = new { Budget = _bidService.GetAutoBidBudget(bidderId), BiddingForItem = _bidService.HasAutoBid(bidderId, itemId) };
            return new JsonResult(result);
        }

        [HttpPost]
        public JsonResult Post(Bid bid)
        {
            lock (_itemService)
            {
                lock (_bidderService)
                {
                    Item item = _itemService.GetItem(bid.ItemId);
                    if (item.LastPrice >= bid.Offer || item.Expiry <= DateTime.Now) return null;
                    Bid addedBid = _bidService.AddBid(bid);
                    return new JsonResult(addedBid);
                }
            }
        }
    }
}
