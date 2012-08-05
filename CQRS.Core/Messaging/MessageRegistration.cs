using System.Reflection;
using Autofac;
using MongoDB.Bson.Serialization;

namespace CQRS.Core.Messaging
{
    public class MessageRegistration<TMessage, TMessageHandler> : MessageRegistration
        where TMessageHandler : IMessageHandler<TMessage>
        where TMessage : class, IMessage
    {
        public override void Apply(ContainerBuilder builder, IContainer container, IBus bus)
        {
            builder.RegisterType<TMessageHandler>();

            bus.RegisterHandler<TMessage>(msg =>
                                              {
                                                  var handler = container.Resolve<TMessageHandler>();
                                                  handler.Handle(msg);
                                              });

            BsonClassMap.RegisterClassMap<TMessage>();
        }
    }

    public abstract class MessageRegistration
    {
        public abstract void Apply(ContainerBuilder builder, IContainer container, IBus bus);
    }
}