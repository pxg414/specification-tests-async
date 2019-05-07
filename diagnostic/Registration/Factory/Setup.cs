using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Specification.Async;

namespace Unity.Specification.Diagnostic.Async.Registration.Factory
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

    public interface IService { }

    public class Service : IService { }

    #endregion

}
