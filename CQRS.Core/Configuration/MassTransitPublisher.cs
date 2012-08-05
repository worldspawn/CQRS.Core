using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EventStore;
using EventStore.Dispatcher;
using Magnum.Reflection;
using MassTransit;

namespace CQRS.Core.Configuration
{
    public class MassTransitPublisher : IBus, IDispatchCommits
    {
        private readonly IServiceBus _bus;

        public MassTransitPublisher(IServiceBus bus)
        {
            _bus = bus;
        }

        #region IBus Members

        void IBus.Send<T>(T command)
        {
            _bus.Publish(command);
        }

        void IBus.RegisterHandler<T>(Action<T> handler)
        {
            _bus.SubscribeHandler(handler);
        }

        #endregion

        #region IDispatchCommits Members

        void IDispatchCommits.Dispatch(Commit commit)
        {
            commit.Events.ForEach(@event => this.PublishEvent(@event.Body));
            //commit.Events.ForEach(@event => { _bus.FastInvoke(new [] {@event.Body.GetType()}, "Publish", @event.Body); });
        }

        public void PublishEvent(object @event)
        {
            //var foo = typeof (PublishExtensions).GetMethods();
            var method = typeof(PublishExtensions).GetMethods().First();
            var generic = method.MakeGenericMethod(@event.GetType());

            generic.Invoke(null, new[] {_bus, @event});
            //_bus.Publish(@event);
        }

        public void Dispose()
        {
            _bus.Dispose();
        }

        #endregion
    }
}