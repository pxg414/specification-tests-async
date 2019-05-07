using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Unity.Specification.Async.Registration.Instance
{
    public abstract partial class SpecificationTests
    {
        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_Null_Async()
        {
            // Act
            Container.RegisterInstance(null, null, null, null);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_Null_Singleton_Async()
        {
            // Act
            Container.RegisterInstance(null, null, null, InstanceLifetime.Singleton);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Empty_Null_Async()
        {
            // Act
            Container.RegisterInstance(Enumerable.Empty<Type>(), null, null, null);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Empty_Null_Singleton_Async()
        {
            // Act
            Container.RegisterInstance(Enumerable.Empty<Type>(), null, null, InstanceLifetime.Singleton);
        }
    }
}
