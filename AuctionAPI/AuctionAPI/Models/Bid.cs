using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BidderId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int Offer { get; set; }
        [Required]
        public DateTime TimeOfBidding { get; set; }
        [Required]
        public DateTime ItemExpiry { get; set; }
        [Required]
        public bool AutoBid { get; set; }
        public Bid() { }
        public Bid(int bidderId,int itemId,int offer,DateTime timeOfBidding,DateTime itemExpiry,bool autoBid=false)
        {
            BidderId = bidderId;
            ItemId = itemId;
            Offer = offer;
            TimeOfBidding = timeOfBidding;
            ItemExpiry = itemExpiry;
            AutoBid = autoBid;
        }
    }
}
