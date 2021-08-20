using AuctionAPI.Interfaces.PublisherSubscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Models
{
    public class PublishSubscribeMiddleMan : IPubSub
    {
        Dictionary<Type, List<ISubscriber>> pubSub = new Dictionary<Type, List<ISubscriber>>();

        public void PublishEvent<Publisher>(Publisher publisher, int bidderId, int itemId, int offer, DateTime expiry)
        {
            Type t = publisher.GetType();
            if (pubSub.TryGetValue(t, out var subscribers))
            {
                subscribers.ForEach(subscriber => subscriber.EventHandlingMethod(bidderId, itemId, offer, expiry));
            }
        }

        public void Subscribe<Publisher>(ISubscriber subscriber)
        {
            Type t = typeof(Publisher);
            if (pubSub.TryGetValue(t, out var subscribers))
            {
                subscribers.Add(subscriber);
            }
            else pubSub.Add(t, new List<ISubscriber> { subscriber });
        }
    }
}
