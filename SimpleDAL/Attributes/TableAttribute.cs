using System;

namespace SimpleDAL
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }

        public TableAttribute()
        { }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }
}
