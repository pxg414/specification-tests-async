using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task UserExceptionIsNotWrappad()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => { throw new System.InvalidOperationException("User error"); });

            // Act
            await Container.Resolve<IService>();
        }
    }
}
