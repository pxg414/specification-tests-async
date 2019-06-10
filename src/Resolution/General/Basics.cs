using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unity.Specification.Resolution.Basics
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public async Task UnityContainerAsync()
        {
            var iuc  = await Container.ResolveAsync(typeof(IUnityContainer));
            var iuca = await Container.ResolveAsync(typeof(IUnityContainerAsync));

            Assert.IsNotNull(iuc);
            Assert.IsNotNull(iuca);

            Assert.AreSame(Container, iuc);
            Assert.AreSame(Container, iuca);
        }

        [TestMethod]
        public async Task CanCreateObjectAsync()
        {
            // Act/Verify
            var instance = await Container.ResolveAsync(typeof(object));
            var repeated = await Container.ResolveAsync(typeof(object));

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(object));
            Assert.AreNotSame(instance, repeated);
        }

        public void MultipleResolvesCreateConcurently()
        {
            var container = GetContainer();
            container.RegisterInstance<string>("a value");

            const int Threads = 40;
            var barrier = new System.Threading.Barrier(Threads);
            var countdown = new CountdownEvent(Threads);
            var random = new Random();
            var errors = false;

            for (int i = 0; i < Threads; i++)
            {
                Task.Factory.StartNew(
                    wait =>
                    {
                        barrier.SignalAndWait();

                        Task.Delay((int)wait).Wait();
                        try
                        {
                            var result = container.ResolveAsync<ClassWithMultipleConstructorParameters>();
                        }
                        catch
                        {
                            errors = true;
                        }

                        countdown.Signal();
                    },
                    random.Next(0, 3),
                    TaskCreationOptions.LongRunning);
            }

            countdown.Wait();
            Assert.IsFalse(errors);
        }

        [TestMethod]
        public void CanWaitAll()
        {
            var results = new[] 
            {
                Container.ResolveAsync(typeof(object)).AsTask(),
                Container.ResolveAsync(typeof(object)).AsTask(),
                Container.ResolveAsync(typeof(object)).AsTask()
            };

            // Act/Verify
            Task.WhenAll(results);

            Assert.IsNotNull(results[0].Result);
            Assert.IsNotNull(results[1].Result);
            Assert.IsNotNull(results[2].Result);
            Assert.IsInstanceOfType(results[0].Result, typeof(object));
            Assert.IsInstanceOfType(results[1].Result, typeof(object));
            Assert.IsInstanceOfType(results[2].Result, typeof(object));
            Assert.AreNotSame(results[0].Result, results[1].Result);
            Assert.AreNotSame(results[1].Result, results[2].Result);
            Assert.AreNotSame(results[0].Result, results[2].Result);
        }

        [TestMethod]
        public async Task ContainerResolvesRecursiveConstructorDependenciesAsync()
        {
            // Act
            var dep = (ObjectWithOneDependency)await Container.ResolveAsync(typeof(ObjectWithOneDependency));

            // Verify
            Assert.IsNotNull(dep);
            Assert.IsNotNull(dep.InnerObject);
            Assert.AreNotSame(dep, dep.InnerObject);
        }

        [TestMethod]
        public async Task ContainerResolvesMultipleRecursiveConstructorDependenciesAsync()
        {
            // Act
            var dep = (ObjectWithTwoConstructorDependencies)await Container.ResolveAsync(typeof(ObjectWithTwoConstructorDependencies));

            // Verify
            dep.Validate();
        }

        [TestMethod]
        public async Task TypeAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterType<IFoo, Foo>(Invoke.Constructor());

            // Act 
            var instance = await Container.ResolveAsync(typeof(IFoo));

            // Validate
            Assert.IsInstanceOfType(instance, typeof(Foo));
        }

        [TestMethod]
        public async Task NamedTypeAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterType<IFoo, Foo>()
                                        .RegisterType<IFoo, Foo1>(Name);

            // Act 
            var instance2 = await Container.ResolveAsync(typeof(IFoo), Name);
            var instance1 = await Container.ResolveAsync(typeof(IFoo));

            // Validate
            Assert.IsInstanceOfType(instance1, typeof(Foo));
            Assert.IsInstanceOfType(instance2, typeof(Foo1));
        }

        [TestMethod]
        public async Task NamedInstanceAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterInstance<IFoo>(new Foo())
                                        .RegisterInstance<IFoo>(Name, new Foo1());

            // Act / Validate
            Assert.IsInstanceOfType(await Container.ResolveAsync(typeof(IFoo)),       typeof(Foo));
            Assert.IsInstanceOfType(await Container.ResolveAsync(typeof(IFoo), Name), typeof(Foo1));
        }

        [TestMethod]
        public async Task NamedFactoryAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IFoo>((c, t, n) => new Foo())
                                        .RegisterFactory<IFoo>(Name, (c, t, n) => new Foo1());

            // Act / Validate
            Assert.IsInstanceOfType(await Container.ResolveAsync(typeof(IFoo)),       typeof(Foo));
            Assert.IsInstanceOfType(await Container.ResolveAsync(typeof(IFoo), Name), typeof(Foo1));
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedTypeNegativeAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterType<IFoo, Foo>()
                                        .RegisterType<IFoo, Foo1>(Name);

            // Act / Validate
            await Container.ResolveAsync(typeof(IFoo), "none");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedInstanceNegativeAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterInstance<IFoo>(new Foo())
                                        .RegisterInstance<IFoo>(Name, new Foo1());

            // Act / Validate
            await Container.ResolveAsync(typeof(IFoo), "none");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public async Task NamedFactoryNegativeAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IFoo>((c, t, n) => new Foo())
                                        .RegisterFactory<IFoo>(Name, (c, t, n) => new Foo1());

            // Act / Validate
            await Container.ResolveAsync(typeof(IFoo), "none");
        }


        [TestMethod]
        public async Task NamedFactoryNegativeTryAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IFoo>((c, t, n) => new Foo())
                                        .RegisterFactory<IFoo>(Name, (c, t, n) => new Foo1());

            // Act / Validate
            try
            {
                var instance = await Container.ResolveAsync(typeof(IFoo), "none");

            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ResolutionFailedException));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UserExceptionIsNotWrappadAsync()
        {
            // Arrange
            ((IUnityContainer)Container).RegisterFactory<IFoo>(c => { throw new System.InvalidOperationException("User error"); });

            // Act
            Container.ResolveAsync<IFoo>();
        }
    }
}
