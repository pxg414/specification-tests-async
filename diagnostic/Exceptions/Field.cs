using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task FieldErrorLevel1()
        {
            Exception exception = null;

            // Act
            try { await Container.Resolve(typeof(ClassWithStringField)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(3, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task FieldErrorLevel2()
        {
            Exception exception = null;

            // Act
            try { await Container.Resolve(typeof(ClassWithStringFieldDependency)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(5, exception.InnerException.Data.Count);
        }


        [TestMethod]
        public async Task FieldWithNamedType()
        {
            Exception exception = null;

            // Act
            try { await Container.Resolve(typeof(ClassWithNamedStringField)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(3, exception.InnerException.Data.Count);
        }

    }
}
