using System;
using CQRS.Core;

namespace UserRegister.Domain.Messages.Events
{
    public class UserCreatedEvent : Event
    {
        public UserCreatedEvent(Guid id, string userName, string firstName, string surname, string email)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            Surname = surname;
            Email = email;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}