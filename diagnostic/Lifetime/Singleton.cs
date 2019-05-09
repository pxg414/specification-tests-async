using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task Singleton_Instance_Null()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterInstance(typeof(IService), null, null, InstanceLifetime.Singleton);

            // Act
            var instance = await Container.Resolve<IService>();

            // Validate
            Assert.IsNull(instance);
            Assert.IsNull(await Container.Resolve<IService>());
        }

        [TestMethod]
        public async Task Singleton_Factory_Null()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => null, FactoryLifetime.Singleton);

            // Act
            var instance = await Container.Resolve<IService>();

            // Validate
            Assert.IsNull(instance);
            Assert.IsNull(await Container.Resolve<IService>());
        }
    }
}
