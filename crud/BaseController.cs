using Microsoft.AspNetCore.Mvc;
using CRUD.Interfaces;

namespace CRUD
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto> : ControllerBase
        where TDto : class, IBaseDto
        where TEntity : class, IBaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TFilterDto : class
    {
        private readonly IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto> _service;

        public BaseController(IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TDto>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TDto>>> GetAll()
        {
            var dtos = await _service.GetAllAsync();
            return Ok(dtos);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<TDto>>> Filter(TFilterDto filter)
        {
            var dtos = await _service.FilterAsync(filter);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<TDto>> Create([FromBody] TCreateDto createDto)
        {
            var dto = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TUpdateDto updateDto)
        {
            var dto = await _service.UpdateAsync(id, updateDto);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
