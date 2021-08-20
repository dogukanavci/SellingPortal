using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Models
{
    public class AutoBid
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BidderId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int Budget { get; set; }
    }
}
