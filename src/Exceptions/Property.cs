using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Async.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task PropertyErrorLevel1()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithStringProperty)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
        }

        [TestMethod]
        public async Task PropertyErrorLevel2()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithStringPropertyDependency)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
        }

        [TestMethod]
        public async Task PropertyWithNamedType()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithNamedStringProperty)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
        }

    }
}
