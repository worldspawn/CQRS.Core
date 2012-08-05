using System;
using EventStore;
using EventStore.Dispatcher;
using Magnum.Reflection;

namespace CQRS.Core.Configuration
{
    public class EasyNetBus : CQRS.Core.IBus, IDispatchCommits
    {
        private readonly EasyNetQ.IBus _bus;

        public EasyNetBus(EasyNetQ.IBus bus)
        {
            _bus = bus;
        }

        public void PublishEvent(object @event)
        {
            using (var publishChannel = _bus.OpenPublishChannel())
            {
                publishChannel.FastInvoke(new[] { @event.GetType() }, "Publish", @event);
            }
        }

        public void Send<T>(T command) where T : class, IMessage
        {
            using (var publishChannel = _bus.OpenPublishChannel())
            {
                publishChannel.FastInvoke(new[] {command.GetType()}, "Publish", command);
            }
        }

        public void RegisterHandler<T>(Action<T> handler) where T : class, IMessage
        {
            _bus.Subscribe(Guid.NewGuid().ToString(), handler);
        }

        public void Dispose()
        {
            //_bus.Dispose();
        }

        void IDispatchCommits.Dispatch(Commit commit)
        {
            commit.Events.ForEach(@event => PublishEvent(@event.Body));
        }
    }
}