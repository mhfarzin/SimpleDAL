using System;

namespace SimpleDAL
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class KeyAttribute : Attribute
    {
        public bool AutoIdentity { get; set; } = true;
    }
}
