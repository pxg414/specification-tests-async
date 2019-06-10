using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Specification.Async;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests : TestFixtureBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        private void ThreadProcedure(object o)
        {
            ThreadInformation info = o as ThreadInformation;

            try
            {
                info.SetThreadResult(Thread.CurrentThread, info.Container.ResolveAsync<IService>());
            }
            catch (System.Exception ex)
            {
                info.SetThreadResult(Thread.CurrentThread, ex);
            }
        }

        public class ThreadInformation
        {
            private readonly object dictLock = new object();

            public ThreadInformation(IUnityContainerAsync container)
            {
                Container = container;
                ThreadResults = new Dictionary<Thread, object>();
            }

            public IUnityContainerAsync Container { get; }

            public Dictionary<Thread, object> ThreadResults { get; }

            public void SetThreadResult(Thread t, object result)
            {
                lock (dictLock)
                {
                    ThreadResults.Add(t, result);
                }
            }
        }
    }

    #region Test Data

    public class TestClass : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }

    public interface IFoo<TEntity>
    {
        TEntity Value { get; }
    }

    public class Foo<TEntity> : IFoo<TEntity>
    {
        public Foo(TEntity value)
        {
            Value = value;
        }

        public TEntity Value { get; }
    }

    public interface IService { }

    public class Service : IService { }

    public class OtherService : IService { }

    public interface IPresenter { }
    public class MockPresenter : IPresenter
    {
        public IView View { get; set; }

        public MockPresenter(IView view)
        {
            View = view;
        }
    }

    public interface IView
    {
        IPresenter Presenter { get; set; }
    }

    public class View : IView
    {
        [Dependency]
        public IPresenter Presenter { get; set; }
    }
    public class SomeService { }

    public class SomeOtherService
    {
        public SomeService SomeService { get; set; }
        public SomeOtherService(SomeService someService)
        {
            this.SomeService = someService;
        }
    }

    public class AService
    {
        public AService(SomeOtherService otherService)
        {
            this.OtherService = otherService;
        }

        [Dependency]
        public SomeService SomeService { get; set; }

        public SomeOtherService OtherService { get; set; }
    }


    public class ObjectWithOneDependency
    {
        private object inner;
        public string id = Guid.NewGuid().ToString();

        public ObjectWithOneDependency(object inner)
        {
            this.inner = inner;
        }

        public object InnerObject
        {
            get { return inner; }
        }

        public void Validate()
        {
            Assert.IsNotNull(inner);
        }
    }

    public class ObjectWithTwoConstructorDependencies
    {
        private ObjectWithOneDependency oneDep;

        public ObjectWithTwoConstructorDependencies(ObjectWithOneDependency oneDep)
        {
            this.oneDep = oneDep;
        }

        public ObjectWithOneDependency OneDep
        {
            get { return oneDep; }
        }

        public void Validate()
        {
            Assert.IsNotNull(oneDep);
            oneDep.Validate();
        }
    }


    #endregion
}
