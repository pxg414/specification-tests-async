using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Unity.Specification.Async.Registration.Factory
{
    public abstract partial class SpecificationTests
    {
        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Null_Null_Null_Async()
        {
            // Act
            await Container.RegisterFactory(null, null, null, null);
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Empty_Null_Null_Async()
        {
            // Act
            await Container.RegisterFactory(Enumerable.Empty<Type>(), null, null, null);
        }
    }
}
