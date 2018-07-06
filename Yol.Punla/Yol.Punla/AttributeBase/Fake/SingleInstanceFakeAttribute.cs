using System;
using System.Diagnostics;

namespace Yol.Punla.AttributeBase
{
    [Conditional("FAKE")]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class SingleInstanceFakeAttribute : Attribute
    {
        public Type Parent { get; set; }

        public SingleInstanceFakeAttribute() { }

        public SingleInstanceFakeAttribute(Type parent) 
        {
            this.Parent = parent;            
        }

    }

}
