using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Unity.Lifetime;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        /// <summary>
        /// Registering var type twice with SetSingleton method. once with default and second with name.
        /// </summary>
        [TestMethod]
        public async Task CheckSetSingletonDoneTwiceAsync()
        {
            await Container.RegisterType<Service>(TypeLifetime.PerContainer);
            await Container.RegisterType<Service>("hello", TypeLifetime.PerContainer);

            var obj  = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
            
            Assert.AreNotSame(obj, obj1);
        }

        [TestMethod]
        [Ignore]
        public async Task CheckSingletonWithDependenciesAsync()
        {
            await Container.RegisterType<ObjectWithOneDependency>(TypeLifetime.PerContainer);

            var result1 = (ObjectWithOneDependency) Container.ResolveAsync<ObjectWithOneDependency>();
            var result2 = (ObjectWithOneDependency) Container.ResolveAsync<ObjectWithOneDependency>();

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result1.InnerObject);
            Assert.IsNotNull(result2.InnerObject);
            Assert.AreSame(result1, result2);
        }

        [TestMethod]
        public async Task CheckSingletonAsDependenciesAsync()
        {
            await Container.RegisterType<ObjectWithOneDependency>(TypeLifetime.PerContainer);

            var result1 = (ObjectWithTwoConstructorDependencies) Container.ResolveAsync<ObjectWithTwoConstructorDependencies>();
            var result2 = (ObjectWithTwoConstructorDependencies) Container.ResolveAsync<ObjectWithTwoConstructorDependencies>();

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result1.OneDep);
            Assert.IsNotNull(result2.OneDep);
            Assert.AreNotSame(result1, result2);
            Assert.AreSame(result1.OneDep, result2.OneDep);
        }

        /// <summary>
        /// Registering var type twice with SetSingleton method. once with default and second with name.
        /// </summary>
        [TestMethod]
        public void CheckRegisterInstanceDoneTwiceAsync()
        {
            var aInstance = new Service();
            ((IUnityContainer)Container).RegisterInstance<Service>(aInstance);
            ((IUnityContainer)Container).RegisterInstance<Service>("hello", aInstance);

            var obj  = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
            
            Assert.AreSame(obj, aInstance);
            Assert.AreSame(obj1, aInstance);
            Assert.AreSame(obj, obj1);
        }

        /// <summary>
        /// Registering var type as singleton and handling its lifetime. Using SetLifetime method.
        /// </summary>
        [TestMethod]
        public async Task SetLifetimeTwiceWithLifetimeHandleAsync()
        {
            await Container.RegisterType<Service>(TypeLifetime.PerContainer);
            await Container.RegisterType<Service>("hello", new HierarchicalLifetimeManager());
            var obj = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
            
            Assert.AreNotSame(obj, obj1);
        }

        /// <summary>
        /// SetSingleton class A. Then register instance of class var twice. once by default second by name.
        /// </summary>
        [TestMethod]
        public void SetSingletonRegisterInstanceTwiceAsync()
        {
            var aInstance = new Service();
            ((IUnityContainer)Container).RegisterInstance(aInstance)
                                        .RegisterInstance("hello", aInstance);

            var obj = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
            
            Assert.AreSame(obj, obj1);
        }

        /// <summary>
        /// SetLifetime class A. Then use GetOrDefault method to get the instances, once without name, second with name.
        /// </summary>
        [TestMethod]
        public async Task SetLifetimeGetTwiceAsync()
        {
            await Container.RegisterType<Service>(TypeLifetime.PerContainer);

            var obj  = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
         
            Assert.AreNotSame(obj, obj1);
        }

        /// <summary>
        /// SetSingleton class A. Then register instance of class var twice. once by default second by name. 
        /// Then SetLifetime once default and then by name.
        /// </summary>
        [TestMethod]
        public void SetSingletonRegisterInstanceTwiceSetLifetimeTwiceAsync()
        {
            var aInstance = new Service();

            ((IUnityContainer)Container).RegisterInstance(aInstance)
                                        .RegisterInstance("hello", aInstance)
                                        .RegisterType<Service>(TypeLifetime.PerContainer)
                                        .RegisterType<Service>("hello1", TypeLifetime.PerContainer);

            var obj = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello1");
            
            Assert.AreNotSame(obj, obj1);
        }

        /// <summary>
        /// SetSingleton class A. Then register instance of class var once by default second by name and
        /// again register instance by another name with lifetime control as false.
        /// Then SetLifetime once default and then by name.
        /// </summary>
        [TestMethod]
        public void SetSingletonNoNameRegisterInstanceDiffNamesAsync()
        {
            var aInstance = new Service();
            ((IUnityContainer)Container).RegisterInstance<Service>(aInstance)
                                        .RegisterInstance<Service>("hello", aInstance)
                                        .RegisterInstance<Service>("hi", aInstance, new ExternallyControlledLifetimeManager());

            var obj = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
            var obj2 = Container.ResolveAsync<Service>("hi");

            Assert.AreSame(obj, obj1);
            Assert.AreSame(obj1, obj2);
        }

        /// <summary>
        /// SetLifetime class A. Then register instance of class var once by default second by name and
        /// again register instance by another name with lifetime control as false.
        /// Then SetLifetime once default and then by name.
        /// </summary>
        [TestMethod]
        public void SetLifetimeNoNameRegisterInstanceDiffNamesAsync()
        {
            var aInstance = new Service();
            ((IUnityContainer)Container).RegisterType<Service>(TypeLifetime.PerContainer)
                                        .RegisterInstance<Service>(aInstance)
                                        .RegisterInstance<Service>("hello", aInstance)
                                        .RegisterInstance<Service>("hi", aInstance, new ExternallyControlledLifetimeManager());

            var obj = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
            var obj2 = Container.ResolveAsync<Service>("hi");
            
            Assert.AreSame(obj, obj1);
            Assert.AreSame(obj1, obj2);
        }

        /// <summary>
        /// SetSingleton class var with name. Then register instance of class var once by default second by name and
        /// again register instance by another name with lifetime control as false.
        /// Then SetLifetime once default and then by name.
        /// </summary>
        [TestMethod]
        public void SetSingletonWithNameRegisterInstanceDiffNamesAsync()
        {
            var aInstance = new Service();
            ((IUnityContainer)Container).RegisterType<Service>("set", TypeLifetime.PerContainer)
                                        .RegisterInstance(aInstance)
                                        .RegisterInstance("hello", aInstance)
                                        .RegisterInstance("hi", aInstance, new ExternallyControlledLifetimeManager());

            var obj = Container.ResolveAsync<Service>("set");
            var obj1 = Container.ResolveAsync<Service>("hello");
            var obj2 = Container.ResolveAsync<Service>("hi");
            
            Assert.AreNotSame(obj, obj1);
            Assert.AreSame(obj1, obj2);
            Assert.AreSame(aInstance, obj1);
        }

        /// <summary>
        /// SetLifetime class var with name. Then register instance of class var once by default second by name and
        /// again register instance by another name with lifetime control as false.
        /// Then SetLifetime once default and then by name.
        /// </summary>
        [TestMethod]
        public void SetLifetimeWithNameRegisterInstanceDiffNamesAsync()
        {
            var aInstance = new Service();
            ((IUnityContainer)Container).RegisterType<Service>("set", TypeLifetime.PerContainer)
                                        .RegisterInstance(aInstance)
                                        .RegisterInstance("hello", aInstance)
                                        .RegisterInstance("hi", aInstance, new ExternallyControlledLifetimeManager());

            var obj = Container.ResolveAsync<Service>("set");
            var obj1 = Container.ResolveAsync<Service>("hello");
            var obj2 = Container.ResolveAsync<Service>("hi");
            
            Assert.AreNotSame(obj, obj1);
            Assert.AreSame(aInstance, obj1);
            Assert.AreSame(obj1, obj2);
        }

        /// <summary>
        /// SetSingleton class A. Then register instance of class var once by default second by name and
        /// lifetime as true. Then again register instance by another name with lifetime control as true
        /// then register.
        /// Then SetLifetime once default and then by name.
        /// </summary>
        [TestMethod]
        public void SetSingletonClassARegisterInstanceOfAandBWithSameNameAsync()
        {
            var aInstance = new Service();
            var bInstance = new OtherService();
            ((IUnityContainer)Container).RegisterType<Service>(TypeLifetime.PerContainer)
                                        .RegisterInstance(aInstance)
                                        .RegisterInstance("hello", aInstance)
                                        .RegisterInstance("hi", bInstance)
                                        .RegisterInstance("hello", bInstance, new ExternallyControlledLifetimeManager());

            var obj = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
            var obj2 = Container.ResolveAsync<OtherService>("hello");
            var obj3 = Container.ResolveAsync<OtherService>("hi");
            
            Assert.AreSame(obj, obj1);
            Assert.AreNotSame(obj, obj2);
            Assert.AreNotSame(obj1, obj2);
            Assert.AreSame(obj2, obj3);
        }

        /// <summary>defect
        /// SetSingleton class var with name. then register instance of var twice. Once by name, second by default.       
        /// </summary>
        [TestMethod]
        public void SetSingletonByNameRegisterInstanceOnitAsync()
        {
            var aInstance = new Service();
            ((IUnityContainer)Container).RegisterType<Service>("SetA", TypeLifetime.PerContainer)
                                        .RegisterInstance<Service>(aInstance)
                                        .RegisterInstance<Service>("hello", aInstance);

            var obj = Container.ResolveAsync<Service>("SetA");
            var obj1 = Container.ResolveAsync<Service>();
            var obj2 = Container.ResolveAsync<Service>("hello");
            
            Assert.AreSame(obj1, obj2);
            Assert.AreNotSame(obj, obj2);
        }

        /// <summary>
        /// Use SetLifetime twice, once with parameter, and without parameter
        /// </summary>
        [TestMethod]
        public void TestSetLifetimeAsync()
        {
            ((IUnityContainer)Container).RegisterType<Service>(TypeLifetime.PerContainer)
                                        .RegisterType<Service>("hello", TypeLifetime.PerContainer);

            var obj = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("hello");
            
            Assert.AreNotSame(obj, obj1);
        }

        /// <summary>
        /// Register class var as singleton then use RegisterInstance to register instance 
        /// of class A.
        /// </summary>
        [TestMethod]
        public void SetSingletonDefaultNameRegisterInstanceAsync()
        {
            var aInstance = new Service();

            ((IUnityContainer)Container).RegisterType(null, typeof(Service), null, TypeLifetime.PerContainer, null)
                                        .RegisterType(null, typeof(Service), "SetA", TypeLifetime.PerContainer, null)
                                        .RegisterInstance(aInstance)
                                        .RegisterInstance("hello", aInstance)
                                        .RegisterInstance("hello", aInstance, new ExternallyControlledLifetimeManager());

            var obj = Container.ResolveAsync<Service>();
            var obj1 = Container.ResolveAsync<Service>("SetA");
            var obj2 = Container.ResolveAsync<Service>("hello");

            Assert.AreNotSame(obj, obj1);
            Assert.AreSame(obj, obj2);
        }

        /// <summary>
        /// Registering var type in both parent as well as child. Now trying to Resolve from both
        /// check if same or different instances are returned.
        /// </summary>
        [TestMethod]
        public async Task RegisterWithParentAndChildAsync()
        {
            //register type UnityTestClass
            var child = Container.CreateChildContainer();

            await Container.RegisterType<Service>(TypeLifetime.PerContainer);
            await child.RegisterType<Service>(TypeLifetime.PerContainer);

            var mytestparent = Container.ResolveAsync<Service>();
            var mytestchild =  child.ResolveAsync<Service>();

            Assert.AreNotSame(mytestparent, mytestchild);
        }

        /// <summary>
        /// Verify WithLifetime managers. When registered using container controlled and freed, even then
        /// same instance is returned when asked for Resolve.
        /// </summary>
        [TestMethod]
        public async Task UseContainerControlledLifetimeAsync()
        {
            await Container.RegisterType<ObjectWithOneDependency>(TypeLifetime.PerContainer);

            var parentinstance = (ObjectWithOneDependency) Container.ResolveAsync<ObjectWithOneDependency>();
            var name = parentinstance.id;
            parentinstance = null;
            GC.Collect();

            var parentinstance1 = (ObjectWithOneDependency) Container.ResolveAsync<ObjectWithOneDependency>();
            Assert.AreEqual(name, parentinstance1.id);
        }

        [TestMethod]
        public void TestStringEmptyAsync()
        {
            ((IUnityContainer)Container).RegisterType<Service>(TypeLifetime.PerContainer)
                                        .RegisterType<Service>(string.Empty, TypeLifetime.PerContainer)
                                        .RegisterType<Service>(null, TypeLifetime.PerContainer);

            var a = Container.ResolveAsync<Service>();
            var b = Container.ResolveAsync<Service>(string.Empty);
            var c = Container.ResolveAsync<Service>((string)null);

            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, c);
            Assert.AreEqual(a, c);
        }
    }
}