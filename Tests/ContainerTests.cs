using System;
using System.Collections.Generic;
using System.Linq;
using Idiocy;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Resolve_Concrete_Type()
        {
            var container = new IdiocyContainer();
            container.Register<Component, IService>();
            var component = container.Resolve<IService>();
            Assert.AreEqual(typeof(Component), component.GetType());
        }


        [Test]
        public void Resolve_Concrete_Type_With_Params()
        {
            var container = new IdiocyContainer();
            container.Register<Component, IService>();
            container.Register<ComponentWithParams, IServiceWithParams>();
            var component = container.Resolve<IServiceWithParams>();

            Assert.AreEqual(typeof(ComponentWithParams), component.GetType());
            Assert.AreEqual(typeof(Component), component.Service.GetType());
        }


        [Test]
        [ExpectedException(typeof(ServiceNotRegisteredException))]
        public void Throw_If_Service_Not_Registered()
        {
            var container = new IdiocyContainer();
            var component = container.Resolve<IServiceWithParams>();
        }


        [Test]
        public void Transient_Instances_Are_Unique()
        {
            var container = new IdiocyContainer();
            container.Register<Component, IService>();
            
            var service1 = container.Resolve<IService>();
            var service2 = container.Resolve<IService>();

            Assert.AreNotSame(service1, service2);
        }


        [Test]
        public void Singleton_Instances_Are_Same()
        {
            var container = new IdiocyContainer();
            container.Register<Component, IService>(Lifetime.Singleton);

            var service1 = container.Resolve<IService>();
            var service2 = container.Resolve<IService>();

            Assert.AreSame(service1, service2);
        }
    }


    internal interface IService { }
    internal class Component : IService { }

    internal interface IServiceWithParams { IService Service { get; } }
    internal class ComponentWithParams : IServiceWithParams
    {
        public ComponentWithParams(IService service) { Service = service; }
        public IService Service { get; private set; }
    }
}
