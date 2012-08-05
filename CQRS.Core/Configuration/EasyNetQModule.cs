using System;
using Autofac;
using EasyNetQ;

namespace CQRS.Core.Configuration
{
    public class EasyNetQModule : Module
    {
        private readonly string _connectionString;

        public EasyNetQModule(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var uri = new Uri(_connectionString);
            string username = null, password = null;

            string easyQString, path = uri.AbsolutePath.TrimStart('/');
            if (path == string.Empty)
                path = "/";

            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                var userInfo = uri.UserInfo.Split(':');
                username = userInfo[0];
                password = userInfo[1];
                easyQString = string.Format("host={0};virtualHost={1};username={2};password={3}", uri.Host,
                                            path, username, password);
            }
            else
            {
                easyQString = string.Format("host={0};virtualHost={1}", uri.Host,
                                            path);
            }
            
            builder.Register(context => RabbitHutch.CreateBus(easyQString)).As<EasyNetQ.IBus>().SingleInstance();
            builder.RegisterType<EasyNetBus>().AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}