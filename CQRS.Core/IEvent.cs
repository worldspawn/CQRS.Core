using System;

namespace CQRS.Core
{
    public interface IEvent : IMessage
    {
        //Guid Id { get; set; }
        Guid SourceId { get; set; }
    }

    public class Event : IEvent
    {
        public Guid SourceId
        {
            get;
            set;
        }
    }
}