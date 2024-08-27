using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tiny.Services;

namespace Tiny.Controllers
{
 
    public class IJsonController<T> : ControllerBase
    where T : new()
    {
        private readonly IJsonFileService<T> _jsonFileService;

        public IJsonController(IJsonFileService<T> jsonFileService)
        {
            _jsonFileService = jsonFileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _jsonFileService.LoadAsync();
            return Ok(data);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] Action<T> updateAction)
        {
            await _jsonFileService.PatchAsync(updateAction);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] T data)
        {
            await _jsonFileService.SaveAsync(data);  // 直接保存传入的数据
            return Ok();
        }



        [HttpGet("query")]
        public async Task<IActionResult> Query([FromQuery] Func<T, bool> queryFunc)
        {
            var result = await _jsonFileService.QueryAsync(queryFunc);
            return Ok(result);
        }
    }
}