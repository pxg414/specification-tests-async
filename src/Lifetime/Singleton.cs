using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void Singleton_Instance_NullAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterInstance(typeof(IService), null, null, InstanceLifetime.Singleton);

            // Act
            var instance = Container.ResolveAsync<IService>();

            // Validate
            Assert.IsNull(instance);
            Assert.IsNull(Container.ResolveAsync<IService>());
        }

        [TestMethod]
        public void Singleton_Factory_NullAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => null, FactoryLifetime.Singleton);

            // Act
            var instance = Container.ResolveAsync<IService>();

            // Validate
            Assert.IsNull(instance);
            Assert.IsNull(Container.ResolveAsync<IService>());
        }
    }
}
