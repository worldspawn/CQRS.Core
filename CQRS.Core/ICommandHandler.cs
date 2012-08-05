namespace CQRS.Core
{
    public interface ICommandHandler<TMessage> : IMessageHandler<TMessage>
    {
    }
}