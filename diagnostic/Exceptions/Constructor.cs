using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task ConstructorErrorLevel0()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(string)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(1, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task ConstructorErrorLevel1()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithStringDependency)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(4, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task ConstructorErrorLevel2()
        {
            Exception exception = null;

            // Act
            try { await Container.ResolveAsync(typeof(ClassWithOtherDependency)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(7, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task ConstructorErrorInterface()
        {
            // Setup
            Exception exception = null;
            ((IUnityContainer)Container).RegisterType<IProvider, Provider>();

            // Act
            try { await Container.ResolveAsync(typeof(IProvider)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(6, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task ConstructorErrorDeep()
        {
            // Setup
            Exception exception = null;
            ((IUnityContainer)Container).RegisterType<IProvider, Provider>();

            // Act
            try { await Container.ResolveAsync(typeof(DependsOnDependOnProvider)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(12, exception.InnerException.Data.Count);
        }
    }
}
