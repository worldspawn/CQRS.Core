using System;
using System.Linq;
using Autofac;

namespace CQRS.Core.Messaging
{
    public static class MessageHost
    {
        public static void RegisterMessageHandlers(IContainer container, params MessageRegistration[] registrations)
        {
            if (container == null) throw new ArgumentNullException("container");
            var bus = container.Resolve<IBus>();

            if (registrations == null || !registrations.Any())
                return;

            var builder = new ContainerBuilder();
            for (int i = 0; i < registrations.Length; i++)
                registrations[i].Apply(builder, container, bus);
            builder.Update(container);
        }

        public static void RegisterMessageHandler<TMessage, TMessageHandler>(IContainer container)
            where TMessageHandler : IMessageHandler<TMessage>
            where TMessage : class, IMessage
        {
            if (container == null) throw new ArgumentNullException("container");
            var bus = container.Resolve<IBus>();

            var builder = new ContainerBuilder();
            new MessageRegistration<TMessage, TMessageHandler>().Apply(builder, container, bus);
            builder.Update(container);
        }
    }
}