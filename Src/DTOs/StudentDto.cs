using CRUD;
using Tiny.Models;

namespace Tiny.DTOs
{
    public class StudentDto : BaseDto, IMapFrom<Student>
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
