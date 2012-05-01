using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core.Registration;
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
            container.Register<IService, Component>();
            var component = container.Resolve<IService>();
            Assert.AreEqual(typeof(Component), component.GetType());
        }


        [Test]
        public void Resolve_Concrete_Type_With_Params()
        {
            var container = new IdiocyContainer();
            container.Register<IService, Component>();
            container.Register<IServiceWithParams, ComponentWithParams>();
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
        public void Autofac_Resolve_Concrete_Type()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Component>().As<IService>();
            var container = builder.Build();

            Assert.AreEqual(typeof(Component), container.Resolve<IService>().GetType());
        }


        [Test]
        public void Autofac_Resolve_Concrete_Type_With_Params()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Component>().As<IService>();
            builder.RegisterType<ComponentWithParams>().As<IServiceWithParams>();
            var container = builder.Build();

            var component = container.Resolve<IServiceWithParams>();

            Assert.AreEqual(typeof(ComponentWithParams), component.GetType());
            Assert.AreEqual(typeof(Component), component.Service.GetType());
        }


        [Test]
        [ExpectedException(typeof(ComponentNotRegisteredException))]
        public void Autofac_Throw_If_Service_Not_Registered()
        {
            var container = new ContainerBuilder().Build();
            var component = container.Resolve<IServiceWithParams>();
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
