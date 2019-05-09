using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task Transient_Factory_Null()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => null);

            // Act
            var instance = await Container.Resolve<IService>();

            // Validate
            Assert.IsNull(instance);
        }
    }
}
