using CQRS.Core;

namespace UserRegister.Domain.Messages.Commands
{
    public class UserCreatedCommand : Command
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}