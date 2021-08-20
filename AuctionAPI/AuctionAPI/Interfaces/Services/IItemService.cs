using AuctionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Data
{
    public interface IItemService
    {
        List<Item> GetItems();
        List<Item> GetItems(BaseGetterObject baseGetterObject);
        int CountItems(string seachTerm);
        Item GetItem(int id);
    }
}
