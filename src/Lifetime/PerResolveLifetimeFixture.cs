using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        [Ignore]
        public async Task ContainerCanBeConfiguredForPerBuildSingleton()
        {
            await Container.RegisterType<IPresenter, MockPresenter>();
            await Container.RegisterType<IView, View>(TypeLifetime.PerResolve);
        }

        [TestMethod]
        public async Task ViewIsReusedAcrossGraph()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterType<IPresenter, MockPresenter>();
            ((IUnityContainer)Container).RegisterType<IView, View>(TypeLifetime.PerResolve);

            var view = (IView) await Container.Resolve<IView>();

            var realPresenter = (MockPresenter)view.Presenter;
            Assert.AreSame(view, realPresenter.View);
        }

        [TestMethod]
        public async Task ViewsAreDifferentInDifferentResolveCalls()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterType<IPresenter, MockPresenter>();
            ((IUnityContainer)Container).RegisterType<IView, View>(TypeLifetime.PerResolve);

            var view1 = (IView) await Container.Resolve<IView>();
            var view2 = (IView) await Container.Resolve<IView>();

            Assert.AreNotSame(view1, view2);
        }

        [TestMethod]
        public async Task PerBuildLifetimeIsHonoredWhenUsingFactory()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterFactory<SomeService>(c => new SomeService(), FactoryLifetime.PerResolve);

            var rootService = (AService) await Container.Resolve<AService>();
            Assert.AreSame(rootService.SomeService, rootService.OtherService.SomeService);
        }
    }
}
