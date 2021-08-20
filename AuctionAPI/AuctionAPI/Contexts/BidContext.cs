using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Models
{
    public class BidContext : DbContext
    {
        public BidContext(DbContextOptions<BidContext> options) : base(options)
        {

        }
        public DbSet<Bid> Bid { get; set; }
    }
}
