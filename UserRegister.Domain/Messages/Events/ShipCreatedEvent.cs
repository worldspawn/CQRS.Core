using System;
using CQRS.Core;

namespace Loveboat.Domain.Messages.Events
{
    public class ShipCreatedEvent : Event
    {
        public ShipCreatedEvent(){}

        public ShipCreatedEvent(Guid shipId, string name, string currentLocation)
        {
            ShipId = shipId;
            Name = name;
            CurrentLocation = currentLocation;
        }

        public Guid ShipId { get; set; }

        public string Name { get; set; }

        public string CurrentLocation { get; set; }
    }
}