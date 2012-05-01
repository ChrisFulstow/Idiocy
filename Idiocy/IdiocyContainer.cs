using System;
using System.Collections.Generic;
using System.Linq;

namespace Idiocy
{
    public class IdiocyContainer
    {
        private readonly Dictionary<Type, Func<object>> _typeRegistry =
            new Dictionary<Type, Func<object>>();

        public void Register<TService, TComponent>() where TComponent : TService
        {
            _typeRegistry.Add(typeof(TService), () => ResolveComponent<TComponent>());
        }

        public TService Resolve<TService>()
        {
            return (TService)Resolve(typeof(TService));
        }

        private object Resolve(Type serviceType)
        {
            Func<object> resolver;
            if (!_typeRegistry.TryGetValue(serviceType, out resolver))
                throw new ServiceNotRegisteredException("The requested service '" + serviceType.FullName + "' has no registered components.");

            return resolver();
        }

        private TComponent ResolveComponent<TComponent>()
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
