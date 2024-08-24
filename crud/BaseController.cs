using CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUD
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TDto, TEntity> : ControllerBase
        where TDto : class, IBaseDto
        where TEntity : class, IBaseEntity
    {
        private readonly IBaseService<TDto, TEntity> _service;

        public BaseController(IBaseService<TDto, TEntity> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var result = await _service.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
