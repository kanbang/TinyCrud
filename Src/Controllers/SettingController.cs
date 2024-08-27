using CRUD;
using CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Tiny.DTOs;
using Tiny.Models;
using Tiny.Services;


namespace Tiny.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController : IJsonController<JsonClass>
    {
        public SettingController(IJsonFileService<JsonClass> jsonFileService) : base(jsonFileService)
        {
        }
    }

}

