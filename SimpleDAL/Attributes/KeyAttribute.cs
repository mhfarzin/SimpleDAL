using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDAL
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class KeyAttribute : Attribute
    {
        public bool AutoIdentity { get; set; } = true;

        public KeyAttribute()
        {
        }
    }
}
