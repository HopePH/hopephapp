using System;

namespace Yol.Punla.AttributeBase
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class SingleInstanceAttribute : Attribute
    {
        public Type Parent { get; set; }

        public SingleInstanceAttribute() { }

        public SingleInstanceAttribute(Type parent) 
        {
            this.Parent = parent;            
        }

    }

}
