using System;
using System.Threading;
using EventStore;
using Magnum.Reflection;

namespace CQRS.Core.Infrastructure
{
    public class ReplayEventStoreCommandHandler : ICommandHandler<ReplayEventStoreCommand>
    {
        private readonly IStoreEvents _storeEvents;
        private readonly IBus _bus;

        public ReplayEventStoreCommandHandler(IStoreEvents storeEvents, IBus bus)
        {
            _storeEvents = storeEvents;
            _bus = bus;
        }

        #region ICommandHandler<DepartureCommand> Members

        public void Handle(ReplayEventStoreCommand message)
        {
            foreach (var commit in _storeEvents.Advanced.GetFrom(new DateTime(2012, 1, 1)))
                foreach (var @event in commit.Events) {
                    _bus.PublishEvent(@event.Body);
                    //Thread.Sleep(500);//TODO: seem to have problems with execution order
                }
        }

        #endregion
    }
}