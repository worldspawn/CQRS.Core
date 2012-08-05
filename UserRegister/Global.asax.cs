using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using CQRS.Core;
using CQRS.Core.Configuration;
using CQRS.Core.Infrastructure;
using CQRS.Core.Messaging;
using CQRS.Core.ViewModel;
using Loveboat.Configuration;
using Loveboat.Domain.CommandHandlers;
using Loveboat.Domain.EventHandlers;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Domain.Messages.Events;
using Loveboat.Domain.ViewModels;
using Loveboat.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SignalR;
using AutofacDependencyResolver = Autofac.Integration.Mvc.AutofacDependencyResolver;

namespace Loveboat
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHubs("~/signalr");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Ships", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            string busEndPointSetting = ConfigurationManager.AppSettings["setting_name_BusEndPointUri"];
            string dbSetting = ConfigurationManager.AppSettings["setting_name_loveboat.db"];
            string busEndPoint = ConfigurationManager.AppSettings[busEndPointSetting];
            string db = ConfigurationManager.AppSettings[dbSetting];

            builder.RegisterModule(new EasyNetQModule(busEndPoint));
            builder.RegisterModule(new EventStoreModule(db));
            builder.RegisterModule(new MongoModule(db));

            builder.RegisterModule(new RepositoryModule(typeof (EventRepository<>),
                                                        typeof (IEventRepository<>)));
            builder.RegisterModule(new RepositoryModule(typeof (DtoRepository<>), typeof (IDtoRepository<>)));

            builder.RegisterType<ViewModelEventDispatcher>().As<IViewModelEventDispatcher>();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(context => { return GlobalHost.DependencyResolver.Resolve<IConnectionManager>(); }).As<IConnectionManager>();

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = new JsonNetSerializer(settings);
            GlobalHost.DependencyResolver.Register(typeof (IJsonSerializer), () => serializer);

            IContainer container = builder.Build();

            //GlobalHost.DependencyResolver = new Configuration.AutofacDependencyResolver(container);
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            try
            {
                //need to make this registration stuff dynamic
                MessageHost.RegisterMessageHandlers(container,
                                                    new MessageRegistration
                                                        <ReplayEventStoreCommand, ReplayEventStoreCommandHandler>(),
                                                    new MessageRegistration<ArrivalCommand, ArrivalCommandHandler>(),
                                                    new MessageRegistration<DepartureCommand, DepartureCommandHandler>(),
                                                    new MessageRegistration
                                                        <ShipCreatedCommand, ShipCreatedCommandHandler>(),
                                                    new MessageRegistration<ExplodingCommand, ExplodedCommandHandler>()
                    );

                MessageHost.RegisterMessageHandlers(container,
                                                    new MessageRegistration<ArrivedEvent, ArrivalEventHandler>(),
                                                    new MessageRegistration<DepartedEvent, DepartedEventHandler>(),
                                                    new MessageRegistration<ShipCreatedEvent, ShipCreatedEventHandler>(),
                                                    new MessageRegistration<ExplodedEvent, ExplodedEventHandler>()
                    );

                MessageHost.RegisterMessageHandlers(container,
                                                    new MessageRegistration
                                                        <ViewModelUpdatedEvent<ShipViewModel>,
                                                        ViewModelUpdatedEventHandler<ShipViewModel>>()
                    );
            }
            catch(Exception ex)
            {
                throw new Exception(busEndPoint, ex);
            }

            //register an update handler for each view model type... preferably dynamically
        }
    }
}