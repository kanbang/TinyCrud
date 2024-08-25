using System.Linq.Expressions;
using AutoMapper;
using CRUD.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUD
{
    public class BaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto> : IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto>
        where TDto : class, IBaseDto
        where TEntity : class, IBaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TFilterDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task<IEnumerable<TDto>> FilterAsync(TFilterDto filter)
        {
            // 将 TFilterDto 转换为一个表达式树
            var predicate = BuildPredicate(filter);

            var entities = await _repository.FilterAsync(predicate);
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task<TDto> CreateAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            await _repository.CreateAsync(entity);
            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> UpdateAsync(int id, TUpdateDto updateDto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Entity not found");

            _mapper.Map(updateDto, entity);
            await _repository.UpdateAsync(entity);
            return _mapper.Map<TDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            // var entity = await _repository.GetByIdAsync(id);
            // if (entity == null)
            //     throw new KeyNotFoundException("Entity not found");

            await _repository.DeleteAsync(id);
        }

        private Expression<Func<TEntity, bool>> BuildPredicate(TFilterDto filter)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            Expression expression = Expression.Constant(true); // 初始条件为 true

            // 获取 TFilterDto 中的所有属性
            var filterProperties = typeof(TFilterDto).GetProperties();

            foreach (var filterProperty in filterProperties)
            {
                var filterValue = filterProperty.GetValue(filter);
                if (filterValue != null)
                {
                    // 获取 TEntity 中对应的同名属性
                    var entityProperty = typeof(TEntity).GetProperty(filterProperty.Name);
                    if (entityProperty != null)
                    {
                        // 创建表达式 entity.PropertyName == filterValue
                        var member = Expression.Property(parameter, entityProperty);
                        var constant = Expression.Constant(filterValue);

                        // 如果属性是字符串类型，使用 Contains 方法，否则使用等于比较
                        Expression comparison;
                        if (entityProperty.PropertyType == typeof(string))
                        {
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            comparison = Expression.Call(member, containsMethod, constant);
                        }
                        else
                        {
                            comparison = Expression.Equal(member, constant);
                        }

                        // 将比较表达式加入最终的查询条件中
                        expression = Expression.AndAlso(expression, comparison);
                    }
                }
            }

            // 生成表达式树的 lambda 表达式
            return Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
        }
    }
}
