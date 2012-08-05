using System;
using Autofac;

namespace CQRS.Core.Configuration
{
    public class RepositoryModule : Module
    {
        private readonly Type _concreteGenericType;
        private readonly Type _serviceType;

        public RepositoryModule(Type concreteGenericType, Type serviceType)
        {
            if (concreteGenericType == null) throw new ArgumentNullException("concreteGenericType");
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            _concreteGenericType = concreteGenericType;
            _serviceType = serviceType;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(_concreteGenericType).As(_serviceType);

            base.Load(builder);
        }
    }
}