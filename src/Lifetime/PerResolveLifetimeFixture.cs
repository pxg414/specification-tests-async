using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        [Ignore]
        public async Task ContainerCanBeConfiguredForPerBuildSingletonAsync()
        {
            await Container.RegisterType<IPresenter, MockPresenter>();
            await Container.RegisterType<IView, View>(TypeLifetime.PerResolve);
        }

        [TestMethod]
        public void ViewIsReusedAcrossGraphAsync()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterType<IPresenter, MockPresenter>();
            ((IUnityContainer)Container).RegisterType<IView, View>(TypeLifetime.PerResolve);

            var view = (IView) Container.ResolveAsync<IView>();

            var realPresenter = (MockPresenter)view.Presenter;
            Assert.AreSame(view, realPresenter.View);
        }

        [TestMethod]
        public void ViewsAreDifferentInDifferentResolveCallsAsync()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterType<IPresenter, MockPresenter>();
            ((IUnityContainer)Container).RegisterType<IView, View>(TypeLifetime.PerResolve);

            var view1 = (IView) Container.ResolveAsync<IView>();
            var view2 = (IView) Container.ResolveAsync<IView>();

            Assert.AreNotSame(view1, view2);
        }

        [TestMethod]
        public void PerBuildLifetimeIsHonoredWhenUsingFactoryAsync()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterFactory<SomeService>(c => new SomeService(), FactoryLifetime.PerResolve);

            var rootService = (AService) Container.ResolveAsync<AService>();
            Assert.AreSame(rootService.SomeService, rootService.OtherService.SomeService);
        }
    }
}
