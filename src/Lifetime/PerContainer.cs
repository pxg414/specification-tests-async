using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task PerContainer_Instance_Null()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterInstance(typeof(IService), null, null, InstanceLifetime.PerContainer);

            // Act
            var instance = await Container.Resolve<IService>();

            // Validate
            Assert.IsNull(instance);
            Assert.IsNull(await Container.Resolve<IService>());
        }

        [TestMethod]
        public async Task PerContainer_Factory_Null()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => null, FactoryLifetime.Singleton);

            // Act
            var instance = await Container.Resolve<IService>();

            // Validate
            Assert.IsNull(instance);
            Assert.IsNull(await Container.Resolve<IService>());
        }

        [TestMethod]
        public async Task PerContainer_ReturnsSameTask()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => new Service(), FactoryLifetime.PerContainer);

            // Act
            var value = ((IUnityContainer)Container).Resolve<IService>();
            var task1 = Container.Resolve<IService>();
            var task2 = Container.Resolve<IService>();

            // Validate
            Assert.AreSame(task1, task2);
            Assert.AreSame(value, await task1);
            Assert.AreSame(value, await task2);
        }

        [TestMethod]
        public async Task PerContainer_ReturnsSameTaskAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => new Service(), FactoryLifetime.PerContainer);

            // Act
            var value = await Container.Resolve<IService>();
            var task1 = Container.Resolve<IService>();
            var task2 = Container.Resolve<IService>();

            // Validate
            Assert.AreSame(task1, task2);
            Assert.AreSame(value, task1.Result);
            Assert.AreSame(value, task2.Result);
        }

        [TestMethod]
        public async Task PerContainer_BuiltOnlyOnceAsync()
        {
            // Arrange
            int count = 0;
            Func<IUnityContainer, Type, string, object> factory = (c,t,n) => 
            {
                Interlocked.Increment(ref count);
                return new Service();
            };
            await Container.RegisterFactory<IService>(factory, FactoryLifetime.PerContainer);

            // Act
            var task = Container.Resolve<IService>();
            var value = ((IUnityContainer)Container).Resolve<IService>();

            // Validate
            Assert.AreEqual(1, count);
            Assert.IsTrue(task.IsCompleted);
            Assert.AreSame(task.Result, value);
        }


        [TestMethod]
        public void PerContainer_ThrowOnFirstResolve()
        {
            // Arrange
            int count = 0;
            bool fail = false;
            Func<IUnityContainer, Type, string, object> factory = (c, t, n) =>
            {
                fail = !fail;
                Interlocked.Increment(ref count);
                return !fail ? new Service() : throw new InvalidOperationException();
            };
            Container.RegisterFactory<IService>(factory, FactoryLifetime.PerContainer)
                     .Wait();

            // Act
            var task = Container.Resolve<IService>();
            var value = ((IUnityContainer)Container).Resolve<IService>();

            // Validate
            Assert.AreSame(Container.Resolve<IService>().Result, value);
        }
    }
}
