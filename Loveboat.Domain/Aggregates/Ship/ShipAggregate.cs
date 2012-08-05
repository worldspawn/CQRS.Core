using System;
using System.Collections.Generic;
using CQRS.Core;
using CQRS.Core.Aggregates;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Domain.Messages.Events;

namespace Loveboat.Domain.Aggregates.Ship
{
    public class ShipAggregate : AggregateBase<IEvent>
    {
        protected string CurrentLocation, Name;

        public ShipAggregate()
        {
            UncommittedEvents = new List<IEvent>();
            MapEvent<DepartedEvent>(OnDeparted);
            MapEvent<ArrivedEvent>(OnArrived);
            MapEvent<ShipCreatedEvent>(OnCreated);
            MapEvent<ExplodedEvent>(OnDeleted);
        }

        private ShipAggregate(string name, string currentLocation) : this()
        {
            ApplyChange(new ShipCreatedEvent(Guid.NewGuid(), name, currentLocation));
        }

        public static ShipAggregate Create(string name, string currentLocation)
        {
            return new ShipAggregate(name, currentLocation);
        }

        public void Depart()
        {
            if (CurrentLocation == "At Sea")
                throw new ApplicationException();

            ApplyChange(new DepartedEvent(Id, "At Sea"));
        }

        public void Arrive(string arrivalPort)
        {
            if (CurrentLocation != "At Sea")
                throw new ApplicationException();

            ApplyChange(new ArrivedEvent(Id, arrivalPort));
        }

        public void Explode()
        {
            ApplyChange(new ExplodedEvent(Id));
        }

        public void OnArrived(ArrivedEvent arrivedEvent)
        {
            CurrentLocation = arrivedEvent.ArrivalPort;
        }

        private void OnDeparted(DepartedEvent departedEvent)
        {
            CurrentLocation = departedEvent.CurrentLocation;
        }

        private void OnCreated(ShipCreatedEvent createdEvent)
        {
            Id = createdEvent.ShipId;
            Name = createdEvent.Name;
            CurrentLocation = createdEvent.CurrentLocation;
        }

        private void OnDeleted(ExplodedEvent explodedEvent)
        {
            CurrentLocation = "Under the sea.";
        }
    }
}