
namespace Unity.Specification.Async
{
    public abstract class TestFixtureBase
    {
        protected IUnityContainerAsync Container;
        protected const string Name = "name";

        public abstract IUnityContainerAsync GetContainer();


        public virtual void Setup()
        {
            Container = GetContainer();
        }
    }
}
