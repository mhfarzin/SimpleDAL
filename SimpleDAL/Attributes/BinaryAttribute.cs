using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDAL
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class BinaryAttribute : Attribute
    {
        public BinaryAttribute()
        { }
    }
}
