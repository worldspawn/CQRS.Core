namespace CQRS.Core
{
    public interface IMessageHandler<TMessage>
    {
        void Handle(TMessage message);
    }
}