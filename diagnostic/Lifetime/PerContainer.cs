using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void PerContainer_Instance_Null()
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
        public void PerContainer_Factory_Null()
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
