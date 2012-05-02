using Autofac;
using Autofac.Core.Registration;
using NUnit.Framework;

namespace Tests
{
    public class AutofacTests
    {
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


        [Test]
        public void Autofac_Transient_Instances_Are_Unique()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Component>().As<IService>().InstancePerDependency();
            var container = builder.Build();

            var service1 = container.Resolve<IService>();
            var service2 = container.Resolve<IService>();

            Assert.AreNotSame(service1, service2);
        }


        [Test]
        public void Autofac_Singleton_Instances_Are_Same()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Component>().As<IService>().SingleInstance();
            var container = builder.Build();

            var service1 = container.Resolve<IService>();
            var service2 = container.Resolve<IService>();

            Assert.AreSame(service1, service2);
        }
    }
}