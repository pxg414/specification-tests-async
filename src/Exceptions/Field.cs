using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Async.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task FieldErrorLevel1()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithStringField)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
        }

        [TestMethod]
        public async Task FieldErrorLevel2()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithStringFieldDependency)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
        }


        [TestMethod]
        public async Task FieldWithNamedType()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithNamedStringField)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
        }

    }
}
