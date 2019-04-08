using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Specification.Async.Registration.Factory
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_Null()
        {
            // Act
            Container.RegisterFactory(null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Empty_Null()
        {
            // Act
            Container.RegisterFactory(Enumerable.Empty<Type>(), null, null, null);
        }
    }
}
