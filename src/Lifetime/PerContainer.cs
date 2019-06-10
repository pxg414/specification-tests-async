using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void PerContainer_InstanceAsyncAsync()
        {
            // Arrange
            var original = new Service();
            ((IUnityContainer)Container).RegisterInstance(typeof(IService), null, original, InstanceLifetime.PerContainer);

            // Act
            var instance = Container.ResolveAsync<IService>();

            // Validate
            Assert.AreSame(original, instance);
        }

        [TestMethod]
        public void PerContainer_InstanceAsync_NullAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterInstance(typeof(IService), null, null, InstanceLifetime.PerContainer);

            // Act
            var instance = Container.ResolveAsync<IService>();

            // Validate
            Assert.IsNull(instance);
            Assert.IsNull(Container.ResolveAsync<IService>());
        }

        [TestMethod]
        public void PerContainer_FactoryAsync_NullAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => null, FactoryLifetime.Singleton);

            // Act
            var instance = Container.ResolveAsync<IService>();

            // Validate
            Assert.IsNull(instance);
            Assert.IsNull(Container.ResolveAsync<IService>());
        }

        [TestMethod]
        public void PerContainer_BuiltOnlyOnceAsync()
        {
            // Arrange
            int count = 0;
            Func<IUnityContainer, Type, string, object> factory = (c,t,n) => 
            {
                Interlocked.Increment(ref count);
                return new Service();
            };
            ((IUnityContainer)Container).RegisterFactory<IService>(factory, FactoryLifetime.PerContainer);

            // Act
            var task = Container.ResolveAsync<IService>();
            var value = ((IUnityContainer)Container).Resolve<IService>();

            // Validate
            Assert.AreEqual(1, count);
            Assert.AreSame(task, value);
        }


        [TestMethod]
        public void PerContainer_ThrowOnFirstResolveAsync()
        {
            // Arrange
            int count = 0;
            bool fail = false;
            Func<IUnityContainer, Type, string, object> factory = (c, t, n) =>
            {
                fail = !fail;
                Interlocked.Increment(ref count);
                return !fail ? new Service() : throw new InvalidOperationException("User Error");
            };
            ((IUnityContainer)Container).RegisterFactory<IService>(factory, FactoryLifetime.PerContainer);

            // Act
            try
            {
                var task = Container.ResolveAsync<IService>();
                Assert.Fail("Should throw");
            }
            catch { /* Ignore */ }

            // Validate
            Assert.AreSame(Container.ResolveAsync<IService>(), 
                ((IUnityContainer)Container).Resolve<IService>());
        }


        [TestMethod]
        public async Task PerContainer_GenericWithInstances()
        {
            ((IUnityContainer)Container).RegisterSingleton(typeof(IFoo<>), typeof(Foo<>));

            var childContainer1 = Container.CreateChildContainer();
            var childContainer2 = Container.CreateChildContainer();

            ((IUnityContainer)childContainer1).RegisterInstance<IService>(new Service());
            ((IUnityContainer)childContainer2).RegisterInstance<IService>(new Service());

            var test1 = await childContainer1.ResolveAsync(typeof(IFoo<object>));
            var test2 = await childContainer2.ResolveAsync(typeof(IFoo<object>));

            Assert.AreSame(test1, test2);
        }

        [TestMethod]
        public void PerContainer_GenericSingletons()
        {
            ((IUnityContainer)Container).RegisterSingleton(typeof(IFoo<>), typeof(Foo<>));

            var childContainer1 = Container.CreateChildContainer();
            var childContainer2 = Container.CreateChildContainer();

            var test1 = childContainer1.ResolveAsync<IFoo<object>>();
            var test2 = childContainer2.ResolveAsync<IFoo<object>>();

            Assert.AreSame(test1, test2);
        }
    }
}
