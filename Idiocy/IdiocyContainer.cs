using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Idiocy
{
    public class IdiocyContainer
    {
        private readonly Dictionary<Type, ComponentRegistration> _componentRegistry =
            new Dictionary<Type, ComponentRegistration>();

        private readonly ConcurrentDictionary<Type,object> _singletons =
            new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Registers a component and the service it provides
        /// </summary>
        public void Register<TComponent, TService>(Lifetime? lifetime = Lifetime.Transient) where TComponent : TService
        {
            _componentRegistry.Add(
                typeof(TService),
                new ComponentRegistration(() => ActivateComponent<TComponent>(), lifetime));
        }


        /// <summary>
        /// Retrieve a service of the requested type from the IoC container
        /// </summary>
        public TService Resolve<TService>()
        {
            return (TService)Resolve(typeof(TService));
        }


        /// <summary>
        /// Retrieve a service of the requested type from the IoC container
        /// </summary>
        public object Resolve(Type serviceType)
        {
            ComponentRegistration componentRegistration;
            if (!_componentRegistry.TryGetValue(serviceType, out componentRegistration))
                throw new ServiceNotRegisteredException("The requested service '" + serviceType.FullName + "' has no registered components.");

            if (componentRegistration.ComponentLifetime == Lifetime.Singleton)
                return _singletons.GetOrAdd(serviceType, t => componentRegistration.Activator());

            return componentRegistration.Activator();
        }


        /// <summary>
        /// Creates a component of the specified type
        /// </summary>
        private TComponent ActivateComponent<TComponent>()
        {
            var constructor = typeof(TComponent).GetConstructors().Single();
            var parameters = constructor.GetParameters().Select(p => Resolve(p.ParameterType));
            var instance = constructor.Invoke(parameters.ToArray());
            return (TComponent)instance;
        }
    }

    public class ServiceNotRegisteredException : Exception
    {
        public ServiceNotRegisteredException(string message) : base(message) { }
    }
}
