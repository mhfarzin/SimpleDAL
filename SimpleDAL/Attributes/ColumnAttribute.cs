using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDAL
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; private set; }

        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }
}
