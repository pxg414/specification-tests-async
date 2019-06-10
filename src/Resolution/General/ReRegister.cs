using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Resolution.Basics
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task RegisteredFactoryOverUnregisterdAsync()
        {
            var instance = new Foo();

            // Act/Verify
            var instance1 = await Container.ResolveAsync(typeof(Foo));
               ((IUnityContainer)Container).RegisterFactory<Foo>((c, t, n) => instance);
            var instance2 = await Container.ResolveAsync(typeof(Foo));

            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);

            Assert.IsInstanceOfType(instance1, typeof(Foo));
            Assert.IsInstanceOfType(instance2, typeof(Foo));

            Assert.AreSame(instance, instance2);
            Assert.AreNotSame(instance1, instance);
            Assert.AreNotSame(instance1, instance2);
        }

        [TestMethod]
        public async Task RegisteredTypeOverUnregisterdAsync()
        {
            // Act/Verify
            var instance1 = await Container.ResolveAsync(typeof(Foo));
            ((IUnityContainer)Container).RegisterType<Foo>(TypeLifetime.PerContainer, Invoke.Constructor());
            var instance2 = await Container.ResolveAsync(typeof(Foo));
            var instance3 = await Container.ResolveAsync(typeof(Foo));

            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsNotNull(instance3);

            Assert.IsInstanceOfType(instance1, typeof(Foo));
            Assert.IsInstanceOfType(instance2, typeof(Foo));
            Assert.IsInstanceOfType(instance3, typeof(Foo));

            Assert.AreNotSame(instance1, instance3);
            Assert.AreNotSame(instance1, instance2);
            Assert.AreSame(instance2, instance3);
        }
    }
}
