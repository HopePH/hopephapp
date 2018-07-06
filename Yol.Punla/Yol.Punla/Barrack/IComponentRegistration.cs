using Prism.Ioc;
using System;

namespace Yol.Punla.Barrack
{
    public interface IComponentRegistration
    {
        void RegisterModuleAsSingleInstance(Type type);
        void RegisterViewsModule(Type type, IContainerRegistry containerRegistry);
        void RegisterModuleAsFactory(Type type);
    }
}
