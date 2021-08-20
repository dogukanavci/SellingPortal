using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Models
{
    public class BaseGetterObject
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
    }
}
