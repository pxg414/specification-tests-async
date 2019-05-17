using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void Transient_Factory_Null()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => null);

            // Act
            var instance = Container.ResolveAsync<IService>();

            // Validate
            Assert.IsNull(instance);
        }
    }
}
