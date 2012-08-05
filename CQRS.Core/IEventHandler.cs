namespace CQRS.Core
{
    public interface IEventHandler<TMessage> : IMessageHandler<TMessage>
    {
    }
}