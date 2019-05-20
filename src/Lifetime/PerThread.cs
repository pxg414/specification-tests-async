using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace Unity.Specification.Lifetime
{
    public abstract partial class SpecificationTests
    {
        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void PerThread_Type_SameThreadAsync()
        {
            ((IUnityContainer)Container).RegisterType<IService, Service>(TypeLifetime.PerThread);

            var a = Container.ResolveAsync<IService>();
            var b = Container.ResolveAsync<IService>();

            Assert.AreSame(a, b);
        }

        [Ignore]
        [TestMethod]
        public void PerThread_Type_DifferentThreadsAsync()
        {
            ((IUnityContainer)Container).RegisterType<IService, Service>(TypeLifetime.PerThread);

            Thread t1 = new Thread(new ParameterizedThreadStart(ThreadProcedure));
            Thread t2 = new Thread(new ParameterizedThreadStart(ThreadProcedure));

            ThreadInformation info = new ThreadInformation(Container);

            t1.Start(info);
            t2.Start(info);
            t1.Join();
            t2.Join();

            var a = info.ThreadResults.Values.FirstOrDefault();
            var b = info.ThreadResults.Values.FirstOrDefault();

            Assert.IsInstanceOfType(a, typeof(Exception));
            Assert.IsInstanceOfType(b, typeof(Exception));
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void PerThread_Factory_SameThreadAsync()
        {
            ((IUnityContainer)Container).RegisterFactory<IService>((c, t, n) => new Service(), FactoryLifetime.PerThread);

            var a = Container.ResolveAsync<IService>();
            var b = Container.ResolveAsync<IService>();
        }

        [TestMethod]
        [Ignore]
        public void PerThread_Factory_DifferentThreadsAsync()
        {
            ((IUnityContainer)Container).RegisterFactory<IService>((c, t, n) => new Service(), FactoryLifetime.PerThread);

            Thread t1 = new Thread(new ParameterizedThreadStart(ThreadProcedure));
            Thread t2 = new Thread(new ParameterizedThreadStart(ThreadProcedure));

            ThreadInformation info = new ThreadInformation(Container);

            t1.Start(info);
            t2.Start(info);
            t1.Join();
            t2.Join();

            var a = info.ThreadResults.Values.FirstOrDefault();
            var b = info.ThreadResults.Values.FirstOrDefault();

            Assert.IsInstanceOfType(a, typeof(Exception));
            Assert.IsInstanceOfType(b, typeof(Exception));
        }
    }
}
