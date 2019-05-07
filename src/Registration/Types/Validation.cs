using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Injection;
using Unity.Lifetime;

namespace Unity.Specification.Async.Registration.Types
{
    public abstract partial class SpecificationTests
    {
        public static IEnumerable<object[]> ArgumetTestData
        {
            get
            {   //                          From                        To                Name  Lifetime                   Verification Data
                yield return new object[] { new[] { typeof(IService) }, typeof(Service),  null, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new[] { typeof(Service)  }, typeof(IService), null, null,                      typeof(TransientLifetimeManager) };

                yield return new object[] { null,                       typeof(object),   null, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new Type[0],                typeof(object),   null, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new[] { (Type)null       }, typeof(object),   null, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new[] { typeof(object)   }, typeof(object),   null, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new[] { typeof(object)   }, typeof(object),   null, null,                      typeof(TransientLifetimeManager) };
                                                                        
                yield return new object[] { null,                       typeof(object),   Name, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new Type[0],                typeof(object),   Name, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new[] { (Type)null       }, typeof(object),   Name, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new[] { typeof(object)   }, typeof(object),   Name, null,                      typeof(TransientLifetimeManager) };
                yield return new object[] { new[] { typeof(object)   }, typeof(object),   Name, null,                      typeof(TransientLifetimeManager) };
                                                                        
                yield return new object[] { null,                       typeof(object),   null, TypeLifetime.PerContainer, typeof(ContainerControlledLifetimeManager) };
                yield return new object[] { new Type[0],                typeof(object),   null, TypeLifetime.PerContainer, typeof(ContainerControlledLifetimeManager) };
                yield return new object[] { new[] { (Type)null       }, typeof(object),   null, TypeLifetime.PerContainer, typeof(ContainerControlledLifetimeManager) };
                yield return new object[] { new[] { typeof(object)   }, typeof(object),   null, TypeLifetime.PerContainer, typeof(ContainerControlledLifetimeManager) };
                yield return new object[] { new[] { typeof(object)   }, typeof(object),   null, TypeLifetime.PerContainer, typeof(ContainerControlledLifetimeManager) };
            }
        }

        public static IEnumerable<object[]> ArgumetTestDataFailing
        {
            get
            {   //                          Exception                  From                  To              Name  Lifetime                   Injection Member
                yield return new object[] { typeof(ArgumentException), null,                 null,           null, null,                      null };
                yield return new object[] { typeof(ArgumentException), null,                 null,           Name, null,                      null };
                yield return new object[] { typeof(ArgumentException), null,                 null,           null, TypeLifetime.PerContainer, null };
                yield return new object[] { typeof(ArgumentException), null,                 null,           Name, TypeLifetime.PerContainer, null };
                                                                                                             
                yield return new object[] { typeof(ArgumentException), new Type[0],          null,           null, null,                      null };
                yield return new object[] { typeof(ArgumentException), new Type[0],          null,           Name, null,                      null };
                yield return new object[] { typeof(ArgumentException), new Type[0],          null,           null, TypeLifetime.PerContainer, null };
                yield return new object[] { typeof(ArgumentException), new Type[0],          null,           Name, TypeLifetime.PerContainer, null };
                                                                                                             
                yield return new object[] { typeof(ArgumentException), new[] { (Type)null }, null,           null, null,                      null };
                yield return new object[] { typeof(ArgumentException), new[] { (Type)null }, null,           Name, null,                      null };
                yield return new object[] { typeof(ArgumentException), new[] { (Type)null }, null,           null, TypeLifetime.PerContainer, null };
            }
        }


        [DataTestMethod]
        [DynamicData(nameof(ArgumetTestData))]
        public async Task ArgumentValidationAsync(IEnumerable<Type> typeFrom, Type typeTo, string name, ITypeLifetimeManager lifetimeManager, Type manager)
        {
            // Act
            await Container.RegisterType(typeFrom, typeTo, name, lifetimeManager);

            // TODO: Add verification 

            //var registeredType = typeFrom ?? typeTo;
            //var registration = Container.Registrations.FirstOrDefault(r => r.RegisteredType == registeredType && r.Name == name);

            //Assert.IsNotNull(registration);
            //Assert.IsInstanceOfType(registration.LifetimeManager, manager);
        }


        [DataTestMethod]
        [DynamicData(nameof(ArgumetTestDataFailing))]
        public async Task ArgumentValidationAsyncFailing(Type exception, IEnumerable<Type> typeFrom, Type typeTo, string name, ITypeLifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            try
            {
                // Act
                await Container.RegisterType(typeFrom, typeTo, name, lifetimeManager, injectionMembers);
                Assert.Fail($"Did not throw and exception of type {exception?.Name}");
            }
            catch (AssertFailedException) { throw; }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, exception);
            }
        }
    }
}
