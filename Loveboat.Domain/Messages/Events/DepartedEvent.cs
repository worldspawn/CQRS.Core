using System;
using CQRS.Core;

namespace Loveboat.Domain.Messages.Events
{
    public class DepartedEvent : Event
    {
        public DepartedEvent(Guid shipId, string currentLocation)
        {
            ShipId = shipId;
            CurrentLocation = currentLocation;
        }

        public string CurrentLocation { get; set; }

        public Guid ShipId { get; set; }
    }
}