using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Models
{
    public class Bidder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
