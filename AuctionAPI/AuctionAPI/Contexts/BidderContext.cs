using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Models
{
    public class BidderContext : DbContext
    {
        public BidderContext(DbContextOptions<BidderContext> options) : base(options)
        {

        }
        public DbSet<Bidder> Bidder { get; set; }
    }
}
