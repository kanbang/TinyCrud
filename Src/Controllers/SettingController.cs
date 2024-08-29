using Microsoft.AspNetCore.Mvc;
using Tiny.DTOs;
using Tiny.Services;

namespace Tiny.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IotConfigController : JsonControllerBase<IotConfig>
    {
        public IotConfigController(IJsonFileService<IotConfig> jsonFileService) : base(jsonFileService)
        {
        }
    }
}
