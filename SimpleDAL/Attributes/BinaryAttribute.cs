using System;

namespace SimpleDAL
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class BinaryAttribute : Attribute
    {
        public BinaryAttribute()
        { }
    }
}
