using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Interfaces.PublisherSubscriber
{
    public interface ISubscriber
    {
        void EventHandlingMethod(int bidderId, int itemId, int offer, DateTime expiry);
    }
}
