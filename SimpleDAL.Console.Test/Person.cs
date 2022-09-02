using SimpleDAL;

namespace SimpleDAL.Console.Test
{
    [Table("Persons")]
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
    }
}
