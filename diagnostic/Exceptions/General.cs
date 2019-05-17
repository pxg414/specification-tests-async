using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Unity.Specification.Diagnostic.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UserExceptionIsNotWrappadAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IService>(c => { throw new System.InvalidOperationException("User error"); });

            // Act
            Container.ResolveAsync<IService>();
        }
    }
}
