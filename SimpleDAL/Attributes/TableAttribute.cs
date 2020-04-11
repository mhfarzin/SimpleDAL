using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDAL
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public string Name { get; private set; }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }
}
