using CRUD;

namespace Tiny.Models
{
    public class Student : BaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

