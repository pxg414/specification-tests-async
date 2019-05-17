using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Unity.Specification.Diagnostic.Lifetime
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
        public void ViewIsReusedAcrossGraph()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterType<IPresenter, MockPresenter>();
            ((IUnityContainer)Container).RegisterType<IView, View>(TypeLifetime.PerResolve);

            var view = (IView) Container.ResolveAsync<IView>();

            var realPresenter = (MockPresenter)view.Presenter;
            Assert.AreSame(view, realPresenter.View);
        }

        [TestMethod]
        public void ViewsAreDifferentInDifferentResolveCalls()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterType<IPresenter, MockPresenter>();
            ((IUnityContainer)Container).RegisterType<IView, View>(TypeLifetime.PerResolve);

            var view1 = Container.ResolveAsync<IView>();
            var view2 = Container.ResolveAsync<IView>();

            Assert.AreNotSame(view1, view2);
        }

        [TestMethod]
        public void PerBuildLifetimeIsHonoredWhenUsingFactory()
        {
            // TODO: Replace with async
            ((IUnityContainer)Container).RegisterFactory<SomeService>(c => new SomeService(), FactoryLifetime.PerResolve);

            var rootService = Container.ResolveAsync<AService>();
            Assert.AreSame(rootService.SomeService, rootService.OtherService.SomeService);
        }
    }
}
