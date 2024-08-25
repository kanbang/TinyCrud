using Microsoft.AspNetCore.Mvc;
using CRUD.Interfaces;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;

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
            // _service = service;
            // Use dynamic filter type if TFilterDto is Default
            var filterDtoType = typeof(TFilterDto) == typeof(TDefault)
                                ? CreateFilterDtoType(typeof(TDto))
                                : typeof(TFilterDto);

            // Initialize service with dynamic filter DTO type
            _service = (IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto>)service;

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


        public static Type CreateFilterDtoType(Type dtoType)
        {
            var assemblyName = new AssemblyName("DynamicFilterDtoAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            // 定义新的类型名称，通常以 "Filter" 后缀命名
            var typeBuilder = moduleBuilder.DefineType($"{dtoType.Name}FilterDto", TypeAttributes.Public | TypeAttributes.Class);

            // 遍历 DTO 类型的所有属性
            foreach (var property in dtoType.GetProperties())
            {
                // 获取属性的类型，如果是值类型，则生成对应的 Nullable 类型
                var propertyType = property.PropertyType;
                if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null)
                {
                    propertyType = typeof(Nullable<>).MakeGenericType(propertyType);
                }

                // 定义新类型的属性
                var fieldBuilder = typeBuilder.DefineField($"_{property.Name.ToLower()}", propertyType, FieldAttributes.Private);
                var propertyBuilder = typeBuilder.DefineProperty(property.Name, PropertyAttributes.HasDefault, propertyType, null);

                // 生成 getter 方法
                var getMethodBuilder = typeBuilder.DefineMethod($"get_{property.Name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
                var getIL = getMethodBuilder.GetILGenerator();
                getIL.Emit(OpCodes.Ldarg_0);
                getIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getIL.Emit(OpCodes.Ret);

                // 生成 setter 方法
                var setMethodBuilder = typeBuilder.DefineMethod($"set_{property.Name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { propertyType });
                var setIL = setMethodBuilder.GetILGenerator();
                setIL.Emit(OpCodes.Ldarg_0);
                setIL.Emit(OpCodes.Ldarg_1);
                setIL.Emit(OpCodes.Stfld, fieldBuilder);
                setIL.Emit(OpCodes.Ret);

                // 连接 getter 和 setter
                propertyBuilder.SetGetMethod(getMethodBuilder);
                propertyBuilder.SetSetMethod(setMethodBuilder);
            }

            // 创建类型
            return typeBuilder.CreateTypeInfo().AsType();
        }

    }

    // public interface ISRD<TItem>: ISRD<TItem, Guid> { }
    public class TDefault { }
    public class BaseController<TDto, TEntity> : BaseController<TDto, TEntity, TDefault, TDefault, TDefault>
            where TDto : class, IBaseDto
            where TEntity : class, IBaseEntity
    {
        public BaseController(IBaseService<TDto, TEntity, TDefault, TDefault, TDefault> service) : base(service)
        {
        }
    }


    public class BaseController<TDto, TEntity, TCreateDto> : BaseController<TDto, TEntity, TCreateDto, TDefault, TDefault>
            where TDto : class, IBaseDto
            where TEntity : class, IBaseEntity
            where TCreateDto : class
    {
        public BaseController(IBaseService<TDto, TEntity, TCreateDto, TDefault, TDefault> service) : base(service)
        {
        }
    }


    public class BaseController<TDto, TEntity, TCreateDto, TUpdateDto> : BaseController<TDto, TEntity, TCreateDto, TUpdateDto, TDefault>
            where TDto : class, IBaseDto
            where TEntity : class, IBaseEntity
            where TCreateDto : class
            where TUpdateDto : class
    {
        public BaseController(IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TDefault> service) : base(service)
        {
        }
    }
}
