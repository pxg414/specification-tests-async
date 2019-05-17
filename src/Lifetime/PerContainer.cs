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
    }
}
