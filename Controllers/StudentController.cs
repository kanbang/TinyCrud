using CRUD;
using CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Tiny.DTOs;
using Tiny.Models;

namespace Tiny.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : BaseController<StudentDto, Student, StudentDto, StudentDto, StudentDto>
    {
        public StudentController(IBaseService<StudentDto, Student, StudentDto, StudentDto, StudentDto> service) : base(service)
        {
        }
    }
}
