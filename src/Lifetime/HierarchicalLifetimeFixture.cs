using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task ThenResolvingInParentActsLikeContainerControlledLifetime()
        {
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = await Container.Resolve<TestClass>();
            var o2 = await Container.Resolve<TestClass>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task ThenParentAndChildResolveDifferentInstances()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = await Container.Resolve<TestClass>();
            var o2 = await child1.Resolve<TestClass>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task ThenChildResolvesTheSameInstance()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = await child1.Resolve<TestClass>();
            var o2 = await child1.Resolve<TestClass>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task ThenSiblingContainersResolveDifferentInstances()
        {
            var child1 = Container.CreateChildContainer();
            var child2 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = await child1.Resolve<TestClass>();
            var o2 = await child2.Resolve<TestClass>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task ThenDisposingOfChildContainerDisposesOnlyChildObject()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = (TestClass) await Container.Resolve<TestClass>();
            var o2 = (TestClass) await child1.Resolve<TestClass>();

            child1.Dispose();
            Assert.IsFalse(o1.Disposed);
            Assert.IsTrue(o2.Disposed);
        }
    }
}
