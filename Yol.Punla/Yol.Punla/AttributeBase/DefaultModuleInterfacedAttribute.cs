using System;

namespace Yol.Punla.AttributeBase
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class DefaultModuleInterfacedAttribute : Attribute
    {
        public Type ParentInterface { get; set; }
    }
}
