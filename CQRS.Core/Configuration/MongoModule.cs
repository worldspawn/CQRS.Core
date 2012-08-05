using System.Configuration;
using Autofac;
using MongoDB.Driver;

namespace CQRS.Core.Configuration
{
    public class MongoModule : Module
    {
        private readonly string _connectionString;

        public MongoModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((context) => MongoDatabase.Create(_connectionString)).SingleInstance();

            base.Load(builder);
        }
    }
}