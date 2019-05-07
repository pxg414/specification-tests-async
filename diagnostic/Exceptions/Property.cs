﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task PropertyErrorLevel1()
        {
            Exception exception = null;

            // Act
            try { await Container.Resolve(typeof(ClassWithStringProperty)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(3, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task PropertyErrorLevel2()
        {
            Exception exception = null;

            // Act
            try { await Container.Resolve(typeof(ClassWithStringPropertyDependency)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(5, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public async Task PropertyWithNamedType()
        {
            Exception exception = null;

            // Act
            try { await Container.Resolve(typeof(ClassWithNamedStringProperty)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(3, exception.InnerException.Data.Count);
        }

    }
}
