using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Models
{
    public class AutoBidContext : DbContext
    {
        public AutoBidContext(DbContextOptions<AutoBidContext> options) : base(options)
        {

        }
        public DbSet<AutoBid> AutoBid { get; set; }
    }
}
