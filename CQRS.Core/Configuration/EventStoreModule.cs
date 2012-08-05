using System;
using Autofac;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Logging;
using EventStore.Persistence;
using EventStore.Persistence.MongoPersistence;
using EventStore.Serialization;

namespace CQRS.Core.Configuration
{
    public class EventStoreModule : Module
    {
        private static readonly byte[] EncryptionKey = new byte[]
                                                           {
                                                               0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa,
                                                               0xb, 0xc, 0xd, 0xe, 0xf
                                                           };

        private readonly AuthorizationPipelineHook _authorizationPipelineHook;

        private readonly string _connectionName;
        private readonly byte[] _encryptionKey;

        public EventStoreModule(string connectionName, byte[] encryptionKey = null)
        {
            if (connectionName == null) throw new ArgumentNullException("connectionName");
            _connectionName = connectionName;
            _encryptionKey = encryptionKey;
            _authorizationPipelineHook = new AuthorizationPipelineHook();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                                 {
                                     var wireup = Wireup.Init();
                                     
                                     return new AdjustedMongoPersistenceWireup(wireup, _connectionName, new DocumentObjectSerializer())
                                         .InitializeStorageEngine()
                                         .UsingJsonSerialization()
                                         .Compress()
                                         .EncryptWith(_encryptionKey ?? EncryptionKey)
                                         .HookIntoPipelineUsing(new[] {_authorizationPipelineHook})
                                         .UsingAsynchronousDispatchScheduler(context.Resolve<IDispatchCommits>())
                                         .Build();
                                 }).As<IStoreEvents>().SingleInstance();

            base.Load(builder);
        }
    }


    public class AdjustedMongoPersistenceFactory : MongoPersistenceFactory
    {
        private readonly string _connectionName;

        public AdjustedMongoPersistenceFactory(string connectionName, IDocumentSerializer serializer) : base(connectionName, serializer)
        {
            _connectionName = connectionName;
        }

        protected override string  GetConnectionString()
        {
            return _connectionName;
        }
    }

    public class AdjustedMongoPersistenceWireup : PersistenceWireup
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(MongoPersistenceWireup));

        static AdjustedMongoPersistenceWireup()
        {
        }

        public AdjustedMongoPersistenceWireup(Wireup inner, string connectionName, IDocumentSerializer serializer)
            : base(inner)
        {
            AdjustedMongoPersistenceWireup.Logger.Debug("Configuring Mongo persistence engine.", new object[0]);
            this.Container.Register(c => new AdjustedMongoPersistenceFactory(connectionName, serializer).Build());
        }
    }

    
}