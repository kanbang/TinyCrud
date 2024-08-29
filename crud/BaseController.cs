using Microsoft.AspNetCore.Mvc;
using CRUD.Interfaces;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;

namespace CRUD
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto> : ControllerBase
        where TDto : class, IBaseDto
        where TEntity : class, IBaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TFilterDto : class
    {
        // private readonly IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto> _service;

        // [Autowired]
        protected IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto> Service { get; set; }

        public BaseController(IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto>? service = null)
        {
            if (service != null)
            {
                Service = service;
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TDto>> GetById(int id)
        {
            var dto = await Service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TDto>>> GetAll()
        {
            var dtos = await Service.GetAllAsync();
            return Ok(dtos);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<TDto>>> Filter(TFilterDto filter)
        {
            var dtos = await Service.FilterAsync(filter);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<TDto>> Create([FromBody] TCreateDto createDto)
        {
            var dto = await Service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TUpdateDto updateDto)
        {
            var dto = await Service.UpdateAsync(id, updateDto);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await Service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await Service.DeleteAsync(id);
            return NoContent();
        }
    }

    // public interface ISRD<TItem>: ISRD<TItem, Guid> { }


    public class BaseController<TDto, TEntity> : BaseController<TDto, TEntity, TDto, TDto, TDto>
            where TDto : class, IBaseDto
            where TEntity : class, IBaseEntity
    {
        public BaseController(IBaseService<TDto, TEntity, TDto, TDto, TDto>? service = null) : base(service)
        {
        }
    }


    public class BaseController<TDto, TEntity, TCreateDto> : BaseController<TDto, TEntity, TCreateDto, TDto, TDto>
            where TDto : class, IBaseDto
            where TEntity : class, IBaseEntity
            where TCreateDto : class
    {
        public BaseController(IBaseService<TDto, TEntity, TCreateDto, TDto, TDto>? service = null) : base(service)
        {
        }
    }


    public class BaseController<TDto, TEntity, TCreateDto, TUpdateDto> : BaseController<TDto, TEntity, TCreateDto, TUpdateDto, TDto>
            where TDto : class, IBaseDto
            where TEntity : class, IBaseEntity
            where TCreateDto : class
            where TUpdateDto : class
    {
        public BaseController(IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TDto>? service = null) : base(service)
        {
        }
    }



    // C#在编译器无法使用反射，无法应用在泛型参数
    class Utils
    {
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

}
