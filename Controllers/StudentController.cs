using CRUD;
using CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Tiny.DTOs;
using Tiny.Models;

namespace Tiny.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : BaseController<StudentDto, Student>
    {
        public StudentController(IBaseService<StudentDto, Student> service) : base(service)
        {
        }
    }
}
