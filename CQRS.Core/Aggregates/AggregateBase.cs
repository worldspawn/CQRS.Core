using System;
using System.Collections.Generic;
using CQRS.Core.Infrastructure;

namespace CQRS.Core.Aggregates
{
    public class AggregateBase<TEvent>
    {
        public IList<TEvent> UncommittedEvents;
        public Guid Id { get; protected set; }
        private readonly IDictionary<Type, Action<TEvent>> _eventMap = new Dictionary<Type, Action<TEvent>>();
        public event Action<TEvent> BeforeEventApplied;

        protected AggregateBase()
        {
        }

        public void LoadFromEvents(Guid id, IEnumerable<TEvent> eventsForAggreate)
        {
            Id = id;
            foreach (var @event in eventsForAggreate)
                ApplyChange(@event, false);
        }

        public void ReplayFromEvents(Guid id, IEnumerable<TEvent> eventsForAggreate)
        {
            Id = id;
            foreach (var @event in eventsForAggreate)
                ApplyChange(@event, true);
        }

        protected void ApplyChange(TEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(TEvent @event, bool isNew)
        {
            Action<TEvent> handler;
            Type eventType = @event.GetType();
            if (!_eventMap.TryGetValue(eventType, out handler))
                throw new UnregisteredDomainEventException(string.Format("The requested domain event '{0}' is not registered in '{1}'", eventType.FullName, GetType().FullName));

            if (BeforeEventApplied != null)
                BeforeEventApplied(@event);
            handler(@event);
            if (isNew) UncommittedEvents.Add(@event);
        }

        protected void MapEvent<TEventType>(Action<TEventType> eventHandler)
            where TEventType : class, TEvent
        {
            _eventMap.Add(typeof(TEventType), theEvent => eventHandler(theEvent as TEventType));
        }
    }
}