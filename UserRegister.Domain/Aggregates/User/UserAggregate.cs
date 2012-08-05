using System;
using System.Collections.Generic;
using CQRS.Core;
using CQRS.Core.Aggregates;
using UserRegister.Domain.Messages.Events;

namespace UserRegister.Domain.Aggregates.User
{
    public class UserAggregate : AggregateBase<IEvent>
    {
        public UserAggregate()
        {
            UncommittedEvents = new List<IEvent>();
            MapEvent<UserCreatedEvent>(OnCreated);
        }

        private UserAggregate(string username, string firstName, string surname, string email) : this()
        {
            ApplyChange(new UserCreatedEvent(Guid.NewGuid(), username, firstName, surname, email));
        }

        internal static UserAggregate Create(string username, string firstName, string surname, string email)
        {
            return new UserAggregate(username, firstName, surname, email);
        }

        private void OnCreated(UserCreatedEvent createdEvent)
        {
            Id = createdEvent.Id;
        }
    }
}