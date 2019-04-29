using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var dep = await Container.Resolve<ObjectWithOneDependency>();

            // Verify
            Assert.IsNotNull(dep);
            Assert.IsNotNull(dep.InnerObject);
            Assert.AreNotSame(dep, dep.InnerObject);
        }

        [TestMethod]
        public async Task ContainerResolvesMultipleRecursiveConstructorDependencies()
        {
            // Act
            var dep = await Container.Resolve<ObjectWithTwoConstructorDependencies>();

            // Verify
            dep.Validate();
        }

        [TestMethod]
        public async Task NamedType()
        {
            // Arrange
            await Container.RegisterType<IFoo, Foo>();
            await Container.RegisterType<IFoo, Foo1>(Name);

            // Act / Validate
            Assert.IsInstanceOfType(await Container.Resolve<IFoo>(), typeof(Foo));
            Assert.IsInstanceOfType(await Container.Resolve<IFoo>(Name), typeof(Foo1));
        }

        [TestMethod]
        public async Task NamedInstance()
        {
            // Arrange
            await Container.RegisterInstance<IFoo>(new Foo());
            await Container.RegisterInstance<IFoo>(Name, new Foo1());

            // Act / Validate
            Assert.IsInstanceOfType(await Container.Resolve<IFoo>(), typeof(Foo));
            Assert.IsInstanceOfType(await Container.Resolve<IFoo>(Name), typeof(Foo1));
        }

        [TestMethod]
        public async Task NamedFactory()
        {
            // Arrange
            await Container.RegisterFactory<IFoo>((c, t, n) => new Foo());
            await Container.RegisterFactory<IFoo>(Name, (c, t, n) => new Foo1());

            // Act / Validate
            Assert.IsInstanceOfType(await Container.Resolve<IFoo>(), typeof(Foo));
            Assert.IsInstanceOfType(await Container.Resolve<IFoo>(Name), typeof(Foo1));
        }


        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedTypeNegative()
        {
            // Arrange
            await Container.RegisterType<IFoo, Foo>();
            await Container.RegisterType<IFoo, Foo1>(Name);

            // Act / Validate
            await Container.Resolve<IFoo>("none");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedInstanceNegative()
        {
            // Arrange
            await Container.RegisterInstance<IFoo>(new Foo());
            await Container.RegisterInstance<IFoo>(Name, new Foo1());

            // Act / Validate
            await Container.Resolve<IFoo>("none");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedFactoryNegative()
        {
            // Arrange
            await Container.RegisterFactory<IFoo>((c, t, n) => new Foo());
            await Container.RegisterFactory<IFoo>(Name, (c, t, n) => new Foo1());

            // Act / Validate
            await Container.Resolve<IFoo>("none");
        }

    }
}
