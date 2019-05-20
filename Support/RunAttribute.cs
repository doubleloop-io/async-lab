using System;

namespace AsyncLab.Support
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RunAttribute : Attribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class DoNotRunAttribute : Attribute
    {
        
    }
}