using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Unity.Specification.Async.Registration.Instance
{
    public abstract partial class SpecificationTests
    {
        //[Ignore]
        //[TestMethod]
        //public void Null_Object_Async()
        //{
        //    var instance = new object();

        //    Container.RegisterInstance(null, null, instance, null);
        //    //            Assert.AreEqual(Container.ResolveAsync<string>(), instance);
        //}

        //[Ignore]
        //[TestMethod]
        //public void Empty_Object_Async()
        //{
        //    var instance = new object();

        //    Container.RegisterInstance(Enumerable.Empty<Type>(), null, instance, null);
        //    //            Assert.AreEqual(Container.ResolveAsync<string>(), instance);
        //}

        //[TestMethod]
        //public void Instance_IUC_NamedObject_Async()
        //{
        //    var instance = Guid.NewGuid().ToString();

        //    Container.RegisterInstance(null, instance, instance, null);

        //    Assert.AreEqual(Container.ResolveAsync<string>(instance), instance);
        //    Assert.ThrowsException<ResolutionFailedException>(() => Container.ResolveAsync<int>());
        //}

        //[TestMethod]
        //public void Instance_IUC_InterfacedObject_Async()
        //{
        //    var instance = new Service();

        //    Container.RegisterInstance(typeof(IService), null, instance, null);

        //    Assert.AreEqual(Container.ResolveAsync<IService>(), instance);
        //    Assert.AreNotEqual(Container.ResolveAsync<Service>(), instance);
        //}

        //[TestMethod]
        //public void Instance_SimpleObject_Async()
        //{
        //    var instance = Guid.NewGuid().ToString();

        //    Container.RegisterInstance(instance);

        //    Assert.AreEqual(Container.ResolveAsync<string>(), instance);
        //}

        //[TestMethod]
        //public void Instance_NamedObject_Async()
        //{
        //    var instance = Guid.NewGuid().ToString();

        //    Container.RegisterInstance(instance, instance);

        //    Assert.AreEqual(Container.ResolveAsync<string>(instance), instance);
        //    Assert.ThrowsException<ResolutionFailedException>(() => Container.ResolveAsync<int>());
        //}

        //[TestMethod]
        //public void Instance_InterfacedObject_Async()
        //{
        //    var instance = new Service();

        //    Container.RegisterInstance<IService>(instance);

        //    Assert.AreEqual(Container.ResolveAsync<IService>(), instance);
        //    Assert.AreNotEqual(Container.ResolveAsync<Service>(), instance);
        //}

        //[TestMethod]
        //public void Instance_NamedInterfacedObject_Async()
        //{
        //    var instance = new Service();
        //    var local = Guid.NewGuid().ToString();

        //    Container.RegisterInstance<IService>(local, instance);

        //    Assert.AreEqual(Container.ResolveAsync<IService>(local), instance);
        //    Assert.AreNotEqual(Container.ResolveAsync<Service>(), instance);
        //    Assert.ThrowsException<ResolutionFailedException>(() => Container.ResolveAsync<IService>(Guid.NewGuid().ToString()));
        //}

        //[TestMethod]
        //public void Instance_ExternallyControlledLifetimeManager_Async()
        //{
        //    var instance = Guid.NewGuid().ToString();

        //    Container.RegisterInstance(instance.GetType(), null, instance, new ExternallyControlledLifetimeManager());
        //    Assert.AreEqual(Container.ResolveAsync<string>(), instance);
        //}


        //[TestMethod]
        //public void Instance_ChainRegistrations_Async()
        //{
        //    var instance = new Service();

        //    Container.RegisterInstance(instance);
        //    Container.RegisterType<IService, Service>();

        //    Assert.AreEqual(Container.ResolveAsync<IService>(), instance);
        //}

        //[TestMethod]
        //public void Instance_RegisterWithParentAndChild_Async()
        //{
        //    //create unity container
        //    var parent = GetContainer();
        //    parent.RegisterInstance(null, null, Guid.NewGuid().ToString(), new ContainerControlledLifetimeManager());

        //    var child = parent.CreateChildContainer();
        //    child.RegisterInstance(null, null, Guid.NewGuid().ToString(), new ContainerControlledLifetimeManager());

        //    Assert.AreSame(parent.ResolveAsync<string>(), parent.ResolveAsync<string>());
        //    Assert.AreSame(child.ResolveAsync<string>(), child.ResolveAsync<string>());
        //    Assert.AreNotSame(parent.ResolveAsync<string>(), child.ResolveAsync<string>());
        //}
    }
}
