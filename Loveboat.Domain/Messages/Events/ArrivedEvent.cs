using System;
using CQRS.Core;
using Loveboat.Domain.Messages.Commands;

namespace Loveboat.Domain.Messages.Events
{
    public class ArrivedEvent : Event
    {
        public string ArrivalPort { get; set; }
        public Guid Id { get; set; }

        public ArrivedEvent(Guid shipId, string arrivalPort)
        {
            if (arrivalPort == null) throw new ArgumentNullException("arrivalPort");
            Id = shipId;
            ArrivalPort = arrivalPort;
        }
    }
}