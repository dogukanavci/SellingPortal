using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Interfaces.PublisherSubscriber
{
    public interface IPubSub
    {
        void Subscribe<Publisher>(ISubscriber subscriber);
        void PublishEvent<Publisher>(Publisher publisher, int bidderId, int itemId, int offer, DateTime expiry);
    }
}
