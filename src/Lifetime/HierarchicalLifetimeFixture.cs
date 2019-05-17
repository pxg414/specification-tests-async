using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void ThenResolvingInParentActsLikeContainerControlledLifetimeAsync()
        {
            ((IUnityContainer)Container).RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = Container.ResolveAsync<TestClass>();
            var o2 = Container.ResolveAsync<TestClass>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task ThenParentAndChildResolveDifferentInstancesAsync()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = Container.ResolveAsync<TestClass>();
            var o2 = child1.ResolveAsync<TestClass>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task ThenChildResolvesTheSameInstanceAsync()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = child1.ResolveAsync<TestClass>();
            var o2 = child1.ResolveAsync<TestClass>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task ThenSiblingContainersResolveDifferentInstancesAsync()
        {
            var child1 = Container.CreateChildContainer();
            var child2 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = child1.ResolveAsync<TestClass>();
            var o2 = child2.ResolveAsync<TestClass>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task ThenDisposingOfChildContainerDisposesOnlyChildObjectAsync()
        {
            var child1 = Container.CreateChildContainer();
            await Container.RegisterType<TestClass>(TypeLifetime.Hierarchical);

            var o1 = Container.ResolveAsync<TestClass>();
            var o2 = child1.ResolveAsync<TestClass>();

            child1.Dispose();
            Assert.IsFalse(o1.Disposed);
            Assert.IsTrue(o2.Disposed);
        }
    }
}
