using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Unity.Specification.Async.Registration.Instance
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_Null()
        {
            // Act
            Container.RegisterInstance(null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_Null_Singleton()
        {
            // Act
            Container.RegisterInstance(null, null, null, InstanceLifetime.Singleton);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Empty_Null()
        {
            // Act
            Container.RegisterInstance(Enumerable.Empty<Type>(), null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Empty_Null_Singleton()
        {
            // Act
            Container.RegisterInstance(Enumerable.Empty<Type>(), null, null, InstanceLifetime.Singleton);
        }
    }
}
