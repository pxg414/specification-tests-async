using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Specification.Async;

namespace Unity.Specification.Diagnostic.Lifetime
{
    public abstract partial class SpecificationTests : TestFixtureBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }
    }

    #region Test Data

    public class Foo
    {
    }

    public interface IService { }

    public class Service : IService { }

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

    #endregion
}
