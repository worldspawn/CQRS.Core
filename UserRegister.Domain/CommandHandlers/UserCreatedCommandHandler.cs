using System;
using CQRS.Core;
using CQRS.Core.Infrastructure;
using UserRegister.Domain.Aggregates.User;
using UserRegister.Domain.Messages.Commands;

namespace UserRegister.Domain.CommandHandlers
{
    public class UserCreatedCommandHandler : ICommandHandler<UserCreatedCommand>
    {
        private readonly IEventRepository<UserAggregate> _eventRepository;

        public UserCreatedCommandHandler(IEventRepository<UserAggregate> eventRepository)
        {
            if (eventRepository == null) throw new ArgumentNullException("eventRepository");
            _eventRepository = eventRepository;
        }

        public void Handle(UserCreatedCommand message)
        {
            var user = UserAggregate.Create(message.Username, message.FirstName, message.Surname, message.Email);
            _eventRepository.Create(user, message.CommandId);
        }
    }
}