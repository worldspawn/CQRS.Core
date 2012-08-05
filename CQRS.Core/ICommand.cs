using System;

namespace CQRS.Core
{
    public interface ICommand : IMessage
    {
        Guid CommandId { get; }
    }

    public abstract class Command : ICommand
    {
        protected Command()
        {
            CommandId = Guid.NewGuid();
        }

        public Guid CommandId { get; set; }
    }
}