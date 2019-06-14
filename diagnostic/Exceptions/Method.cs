using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task MethodErrorLevel1()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithMethod)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(4, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task MethodErrorLevel2()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassDependingOnMethod)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(7, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task MethodWithOutParam()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithOutMethod)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(4, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task MethodWithRefParam()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithRefMethod)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(4, exception.InnerException.Data.Count);
        }
    }
}
