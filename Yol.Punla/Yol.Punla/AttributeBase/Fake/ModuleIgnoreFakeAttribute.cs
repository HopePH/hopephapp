using System;
using System.Diagnostics;

namespace Yol.Punla.AttributeBase
{
    [Conditional("FAKE")]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class ModuleIgnoreFakeAttribute : Attribute
    {
    }

}
