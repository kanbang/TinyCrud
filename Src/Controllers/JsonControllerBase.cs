using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tiny.Services;

namespace Tiny.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class JsonControllerBase<T> : ControllerBase where T : class, new()
    {
        private readonly IJsonFileService<T> _jsonFileService;

        public JsonControllerBase(IJsonFileService<T> jsonFileService)
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
        public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<T> patchDoc)
        {
            await _jsonFileService.PatchAsync(patchDoc);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] T data)
        {
            await _jsonFileService.SaveAsync(data);
            return Ok();
        }

        [HttpGet("query")]
        public async Task<IActionResult> Query([FromQuery] string jsonPath)
        {
            var result = await _jsonFileService.QueryAsync<object>(jsonPath);
            return Ok(result);
        }


        [HttpPost("generate-patch")]
        public async Task<IActionResult> GeneratePatch([FromBody] T newData)
        {
            var patchDoc = await _jsonFileService.GenerateJsonPatchAsync(newData);
            return Ok(patchDoc);
        }

    }
}
