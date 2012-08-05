using System;
using CQRS.Core;

namespace Loveboat.Domain.Messages.Events
{
    public class ExplodedEvent : Event
    {
        public ExplodedEvent(Guid shipId)
        {
            Id = shipId;
        }

        public Guid Id { get; set; }
    }
}