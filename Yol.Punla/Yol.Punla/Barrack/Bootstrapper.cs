using Prism.Unity;
using Yol.Punla.AttributeBase;
using System;
using System.Reflection;
using System.Linq;
using Unity.Lifetime;
using Unity.Registration;
using Prism.Ioc;
using Unity;
using System.Diagnostics;

namespace Yol.Punla.Barrack
{
    public static class Bootstrapper
    {
        private static IUnityContainer Container;
        private static IContainerRegistry ContainerRegistry;

        public static void InitContainer(IContainerRegistry containerRegistry)
        {
            ContainerRegistry = containerRegistry;
            Container = containerRegistry.GetContainer();
        }

        public static void AutoTypesRegistration<T>()
        {
            RegisterViewsModule(typeof(T));
            RegisterModuleAsSingleInstance(typeof(T));
            RegisterModuleAsFactory(typeof(T));

            RegisterViewsModuleFAKE(typeof(T));
            RegisterModuleAsSingleInstanceFAKE(typeof(T));
            RegisterModuleAsFactoryFAKE(typeof(T));
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private static void RegisterModuleAsFactory(Type type)
        {
            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<FactoryModuleAttribute>() != null))
                Container.RegisterType(exportedType.AsType(), new TransientLifetimeManager(), new InjectionMember[0]);
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private static void RegisterModuleAsSingleInstance(Type type)
        {
            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<DefaultModuleAttribute>() != null))
                Container.RegisterType(exportedType.AsType(), new ContainerControlledLifetimeManager(), new InjectionMember[0]);

            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<DefaultModuleInterfacedAttribute>() != null))
            {
                var parentInterface = exportedType.GetCustomAttribute<DefaultModuleInterfacedAttribute>().ParentInterface;

                if (parentInterface == null)
                    parentInterface = exportedType.ImplementedInterfaces.First();

                Container.RegisterType(parentInterface, exportedType.AsType(), new ContainerControlledLifetimeManager(), new InjectionMember[0]);
            }
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private static void RegisterViewsModule(Type type)
        {
            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<ModuleViewAttribute>() != null))
                ContainerRegistry.RegisterForNavigation(exportedType.AsType(), exportedType.AsType().Name);
        }

        [Conditional("FAKE")]
        private static void RegisterModuleAsFactoryFAKE(Type type)
        {
            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<FactoryModuleFakeAttribute>() != null))
                Container.RegisterType(exportedType.AsType(), new TransientLifetimeManager(), new InjectionMember[0]);
        }

        [Conditional("FAKE")]
        private static void RegisterModuleAsSingleInstanceFAKE(Type type)
        {
            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<DefaultModuleFakeAttribute>() != null))
                Container.RegisterType(exportedType.AsType(), new ContainerControlledLifetimeManager(), new InjectionMember[0]);

            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<DefaultModuleInterfacedFakeAttribute>() != null))
            {
                var parentInterface = exportedType.GetCustomAttribute<DefaultModuleInterfacedFakeAttribute>().ParentInterface;

                if (parentInterface == null)
                    parentInterface = exportedType.ImplementedInterfaces.First();

                Container.RegisterType(parentInterface, exportedType.AsType(), new ContainerControlledLifetimeManager(), new InjectionMember[0]);
            }
        }

        [Conditional("FAKE")]
        private static void RegisterViewsModuleFAKE(Type type)
        {
            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<ModuleViewAttribute>() != null))
                ContainerRegistry.RegisterForNavigation(exportedType.AsType(), exportedType.AsType().Name);
        }
    }
}
