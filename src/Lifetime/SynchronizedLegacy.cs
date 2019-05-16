using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task LegacyCheckSetSingletonDoneTwice()
        {
            await Container.RegisterType<Service>(new LegacyPerContainer());
            await Container.RegisterType<Service>("hello", new LegacyPerContainer());

            var obj  = await Container.Resolve<Service>();
            var obj1 = await Container.Resolve<Service>("hello");

            Assert.AreNotSame(obj, obj1);
        }

        [TestMethod]
        public async Task LegacyCheckSingletonWithDependencies()
        {
            await Container.RegisterType<ObjectWithOneDependency>(new LegacyPerContainer());

            var result1 = (ObjectWithOneDependency) await Container.Resolve<ObjectWithOneDependency>();
            var result2 = (ObjectWithOneDependency) await Container.Resolve<ObjectWithOneDependency>();

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result1.InnerObject);
            Assert.IsNotNull(result2.InnerObject);
            Assert.AreSame(result1, result2);
        }

        [TestMethod]
        public async Task LegacyCheckSingletonAsDependencies()
        {
            await Container.RegisterType<ObjectWithOneDependency>(new LegacyPerContainer());

            var result1 = (ObjectWithTwoConstructorDependencies) await Container.Resolve<ObjectWithTwoConstructorDependencies>();
            var result2 = (ObjectWithTwoConstructorDependencies) await Container.Resolve<ObjectWithTwoConstructorDependencies>();

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result1.OneDep);
            Assert.IsNotNull(result2.OneDep);
            Assert.AreNotSame(result1, result2);
            Assert.AreSame(result1.OneDep, result2.OneDep);
        }

        [TestMethod]
        [Ignore]
        public async Task LegacyCheckRegisterInstanceDoneTwice()
        {
            var aInstance = new Service();
            await Container.RegisterInstance<Service>(aInstance);
            await Container.RegisterInstance<Service>("hello", aInstance);

            var obj  = await Container.Resolve<Service>();
            var obj1 = await Container.Resolve<Service>("hello");

            Assert.AreSame(obj, aInstance);
            Assert.AreSame(obj1, aInstance);
            Assert.AreSame(obj, obj1);
        }

        [TestMethod]
        public async Task LegacySetLifetimeTwiceWithLifetimeHandle()
        {
            await Container.RegisterType<Service>(new LegacyPerContainer());
            await Container.RegisterType<Service>("hello", new LegacyHierarchical());

            var obj  = await Container.Resolve<Service>();
            var obj1 = await Container.Resolve<Service>("hello");

            Assert.AreNotSame(obj, obj1);
        }

        [TestMethod]
        [Ignore]
        public async Task LegacySetSingletonRegisterInstanceTwice()
        {
            var aInstance = new Service();
            await Container.RegisterInstance<Service>(aInstance);
            await Container.RegisterInstance<Service>("hello", aInstance);

            var obj  = await Container.Resolve<Service>();
            var obj1 = await Container.Resolve<Service>("hello");

            Assert.AreSame(obj, obj1);
        }

        [TestMethod]
        public async Task LegacySetLifetimeGetTwice()
        {
            await Container.RegisterType<Service>(new LegacyPerContainer());
            var obj  = await Container.Resolve<Service>();
            var obj1 = await Container.Resolve<Service>("hello");

            Assert.AreNotSame(obj, obj1);
        }

        [TestMethod]
        [Ignore]
        public async Task LegacySetSingletonRegisterInstanceTwiceSetLifetimeTwice()
        {
            var aInstance = new Service();

            await Container.RegisterInstance(aInstance);
            await Container.RegisterInstance("hello", aInstance);
            await Container.RegisterType<Service>(new LegacyPerContainer());
            await Container.RegisterType<Service>("hello1", new LegacyPerContainer());

            var obj  = await Container.Resolve<Service>();
            var obj1 = await Container.Resolve<Service>("hello1");

            Assert.AreNotSame(obj, obj1);
        }

        [TestMethod]
        [Ignore]
        public async Task LegacySetSingletonByNameRegisterInstanceOnit()
        {
            var aInstance = new Service();
            await Container.RegisterType<Service>("SetA", new LegacyPerContainer());
            await Container.RegisterInstance<Service>(aInstance);
            await Container.RegisterInstance<Service>("hello", aInstance);

            var obj  = await Container.Resolve<Service>("SetA");
            var obj1 = await Container.Resolve<Service>();
            var obj2 = await Container.Resolve<Service>("hello");

            Assert.AreSame(obj1, obj2);
            Assert.AreNotSame(obj, obj2);
        }

        [TestMethod]
        public async Task LegacyTestSetLifetime()
        {
            await Container.RegisterType<Service>(new LegacyPerContainer());
            await Container.RegisterType<Service>("hello", new LegacyPerContainer());

            var obj  = await Container.Resolve<Service>();
            var obj1 = await Container.Resolve<Service>("hello");

            Assert.AreNotSame(obj, obj1);
        }

        [TestMethod]
        public async Task LegacyRegisterWithParentAndChild()
        {
            //register type UnityTestClass
            var child = Container.CreateChildContainer();

            await Container.RegisterType<Service>(new LegacyPerContainer());
            await child.RegisterType<Service>(new LegacyPerContainer());

            var mytestparent = await Container.Resolve<Service>();
            var mytestchild  = await child.Resolve<Service>();

            Assert.AreNotSame(mytestparent, mytestchild);
        }

        [TestMethod]
        public async Task LegacyUseContainerControlledLifetime()
        {
            await Container.RegisterType<Service>(new LegacyPerContainer());

            var parentinstance = await Container.Resolve<Service>();
            var hash = parentinstance.GetHashCode();
            parentinstance = null;
            GC.Collect();

            var parentinstance1 = await Container.Resolve<Service>();
            Assert.AreEqual(hash, parentinstance1.GetHashCode());
        }

        [TestMethod]
        public async Task LegacyTestStringEmpty()
        {
            await Container.RegisterType<Service>(new LegacyPerContainer());
            await Container.RegisterType<Service>(string.Empty, new LegacyPerContainer());
            await Container.RegisterType<Service>(null, new LegacyPerContainer());

            Service a = (Service) await Container.Resolve<Service>();
            Service b = (Service) await Container.Resolve<Service>(string.Empty);
            Service c = (Service) await Container.Resolve<Service>((string)null);

            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, c);
            Assert.AreEqual(a, c);
        }

        [TestMethod]
        public async Task LegacyThenResolvingInParentActsLikeContainerControlledLifetime()
        {
            await Container.RegisterType<TestClass>(new LegacyHierarchical());

            var o1 = await Container.Resolve<TestClass>();
            var o2 = await Container.Resolve<TestClass>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task LegacyThenParentAndChildResolveDifferentInstances()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(new LegacyHierarchical());

            var o1 = await Container.Resolve<TestClass>();
            var o2 = await child1.Resolve<TestClass>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task LegacyThenChildResolvesTheSameInstance()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(new LegacyHierarchical());

            var o1 = await child1.Resolve<TestClass>();
            var o2 = await child1.Resolve<TestClass>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task LegacyThenSiblingContainersResolveDifferentInstances()
        {
            var child1 = Container.CreateChildContainer();
            var child2 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(new LegacyHierarchical());

            var o1 = await child1.Resolve<TestClass>();
            var o2 = await child2.Resolve<TestClass>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task LegacyThenDisposingOfChildContainerDisposesOnlyChildObject()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(new LegacyHierarchical());

            var o1 = (TestClass) await Container.Resolve<TestClass>();
            var o2 = (TestClass) await child1.Resolve<TestClass>();

            child1.Dispose();
            Assert.IsFalse(o1.Disposed);
            Assert.IsTrue(o2.Disposed);
        }


        [TestMethod]
        [Ignore]
        public void LegacySameInstanceFromMultipleThreads()
        {
            //await Container.RegisterFactory<IService>((c, t, n) => new Service(), new LegacyPerContainer());

            //object result1 = null;
            //object result2 = null;

            //Thread thread1 = new Thread(delegate ()
            //{
            //    result1 = await Container.Resolve<IService>();
            //});

            //Thread thread2 = new Thread(delegate ()
            //{
            //    result2 = await Container.Resolve<IService>();
            //});

            //thread1.Name = "1";
            //thread2.Name = "2";

            //thread1.Start();
            //thread2.Start();

            //thread2.Join();
            //thread1.Join();

            //Assert.IsNotNull(result1);
            //Assert.AreSame(result1, result2);
        }

        [TestMethod]
        [Ignore]
        public void LegacyContainerControlledLifetimeDoesNotLeaveHangingLockIfBuildThrowsException()
        {
            //int count = 0;
            //bool fail = false;
            //Func<IUnityContainer, Type, string, object> factory = (c, t, n) =>
            //{
            //    fail = !fail;
            //    Interlocked.Increment(ref count);
            //    return !fail ? new Service() : throw new InvalidOperationException();
            //};
            //await Container.RegisterFactory<IService>(factory, new LegacyPerContainer());

            //object result1 = null;
            //object result2 = null;
            //bool thread2Finished = false;

            //Thread thread1 = new Thread(
            //    delegate ()
            //    {
            //        try
            //        {
            //            result1 = await Container.Resolve<IService>();
            //        }
            //        catch (ResolutionFailedException)
            //        {
            //        }
            //    });

            //Thread thread2 = new Thread(
            //    delegate ()
            //    {
            //        result2 = await Container.Resolve<IService>();
            //        thread2Finished = true;
            //    });

            //thread1.Start();
            //thread1.Join();

            //// Thread1 threw an exception. However, lock should be correctly freed.
            //// Run thread2, and if it finished, we're ok.

            //thread2.Start();
            //thread2.Join(500);
            ////thread2.Join();

            //Assert.IsTrue(thread2Finished);
            //Assert.IsNull(result1);
            //Assert.IsNotNull(result2);
        }
    }
}
