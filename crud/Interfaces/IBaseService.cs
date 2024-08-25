namespace CRUD.Interfaces
{
    public interface IBaseService<TDto, TEntity, TCreateDto, TUpdateDto, TFilterDto>
        where TDto : class, IBaseDto
        where TEntity : class, IBaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TFilterDto : class
    {
        Task<TDto> GetByIdAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<IEnumerable<TDto>> FilterAsync(TFilterDto filter);
        Task<TDto> CreateAsync(TCreateDto createDto);
        Task<TDto> UpdateAsync(int id, TUpdateDto updateDto);
        Task DeleteAsync(int id);
    }
}
