using AuctionAPI.Interfaces.PublisherSubscriber;
using AuctionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Data
{
    public class ItemService : IItemService, ISubscriber
    {
        private ItemContext _itemContext;
        public ItemService(ItemContext itemContext, IPubSub publishSubscribe)
        {
            publishSubscribe.Subscribe<BidService>(this);
            _itemContext = itemContext;
        }
        public void EventHandlingMethod(int bidderId, int itemId, int offer, DateTime expiry)
        {
            Item item = _itemContext.Item.Where(i => i.Id == itemId).FirstOrDefault();
            if( item != null) item.LastPrice = offer;
            _itemContext.SaveChanges();
        }
        public List<Item> GetItems()
        {
            return _itemContext.Item.Where(i => i.Expiry > DateTime.Now).ToList();
        }
        public Item GetItem(int id)
        {
            return GetItems().Where(i => i.Id == id).FirstOrDefault();
        }

        private IEnumerable<Item> _searchItemsPerBaseGetter(string searchTerm)
        {
            var searchedResult = GetItems().Where(i => i.Name.Contains(searchTerm) || i.Description.Contains(searchTerm));
            
            return searchedResult;
        }

        public List<Item> GetItems(BaseGetterObject baseGetterObject)
        {
            var searchedResult = _searchItemsPerBaseGetter(baseGetterObject.SearchTerm);
            switch (baseGetterObject.SortBy)
            {
                case "Name":
                    searchedResult = searchedResult.OrderBy(i => i.Name);
                    break;
                case "Description":
                    searchedResult = searchedResult.OrderBy(i => i.Description);
                    break;
                case "Expiry":
                    searchedResult = searchedResult.OrderBy(i => i.Expiry);
                    break;
                case "Bid":
                    searchedResult = searchedResult.OrderByDescending(i => i.LastPrice);
                    break;
                default:
                    break;
            }

            int offset = baseGetterObject.PageSize * Math.Max(0, baseGetterObject.PageIndex - 1);
            return searchedResult.Skip(offset).Take(baseGetterObject.PageSize).ToList();
        }

        public int CountItems(string searchTerm)
        {
            return _searchItemsPerBaseGetter(searchTerm).Count();
        }

    }
}
