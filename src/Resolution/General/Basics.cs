using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Unity.Specification.Resolution.Basics
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task UnityContainer()
        {
            var iuc  = await Container.Resolve(typeof(IUnityContainer));
            var iuca = await Container.Resolve(typeof(IUnityContainerAsync));

            Assert.IsNotNull(iuc);
            Assert.IsNotNull(iuca);

            Assert.AreSame(Container, iuc);
            Assert.AreSame(Container, iuca);
        }

        [TestMethod]
        public async Task CanCreateObject()
        {
            // Act/Verify
            var instance = await Container.Resolve(typeof(object));
            var repeated = await Container.Resolve(typeof(object));

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(object));
        }

        [TestMethod]
        public async Task ContainerResolvesRecursiveConstructorDependencies()
        {
            // Act
            var dep = (ObjectWithOneDependency)await Container.Resolve(typeof(ObjectWithOneDependency));

            // Verify
            Assert.IsNotNull(dep);
            Assert.IsNotNull(dep.InnerObject);
            Assert.AreNotSame(dep, dep.InnerObject);
        }

        [TestMethod]
        public async Task ContainerResolvesMultipleRecursiveConstructorDependencies()
        {
            // Act
            var dep = (ObjectWithTwoConstructorDependencies)await Container.Resolve(typeof(ObjectWithTwoConstructorDependencies));

            // Verify
            dep.Validate();
        }

        [TestMethod]
        public async Task NamedType()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterType<IFoo, Foo>()
                                        .RegisterType<IFoo, Foo1>(Name);

            // Act 
            var instance1 = await Container.Resolve(typeof(IFoo));
            var instance2 = await Container.Resolve(typeof(IFoo), Name);

            // Validate
            Assert.IsInstanceOfType(instance1, typeof(Foo));
            Assert.IsInstanceOfType(instance2, typeof(Foo1));
        }

        [TestMethod]
        public async Task NamedInstance()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterInstance<IFoo>(new Foo())
                                        .RegisterInstance<IFoo>(Name, new Foo1());

            // Act / Validate
            Assert.IsInstanceOfType(await Container.Resolve(typeof(IFoo)),       typeof(Foo));
            Assert.IsInstanceOfType(await Container.Resolve(typeof(IFoo), Name), typeof(Foo1));
        }

        [TestMethod]
        public async Task NamedFactory()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IFoo>((c, t, n) => new Foo())
                                        .RegisterFactory<IFoo>(Name, (c, t, n) => new Foo1());

            // Act / Validate
            Assert.IsInstanceOfType(await Container.Resolve(typeof(IFoo)),       typeof(Foo));
            Assert.IsInstanceOfType(await Container.Resolve(typeof(IFoo), Name), typeof(Foo1));
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedTypeNegative()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterType<IFoo, Foo>()
                                        .RegisterType<IFoo, Foo1>(Name);

            // Act / Validate
            await Container.Resolve(typeof(IFoo), "none");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedInstanceNegative()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterInstance<IFoo>(new Foo())
                                        .RegisterInstance<IFoo>(Name, new Foo1());

            // Act / Validate
            await Container.Resolve(typeof(IFoo), "none");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedFactoryNegative()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IFoo>((c, t, n) => new Foo())
                                        .RegisterFactory<IFoo>(Name, (c, t, n) => new Foo1());

            // Act / Validate
            await Container.Resolve(typeof(IFoo), "none");
        }


        [TestMethod]
        public async Task NamedFactoryNegativeTry()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IFoo>((c, t, n) => new Foo())
                                        .RegisterFactory<IFoo>(Name, (c, t, n) => new Foo1());

            // Act / Validate
            try
            {
                var instance = await Container.Resolve(typeof(IFoo), "none");

            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ResolutionFailedException));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task UserExceptionIsNotWrappad()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IFoo>(c => { throw new System.InvalidOperationException("User error"); });

            // Act
            await Container.Resolve<IFoo>();
        }
    }
}
