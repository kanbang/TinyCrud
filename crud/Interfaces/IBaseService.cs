namespace CRUD.Interfaces
{
    public interface IBaseService<TDto, TEntity>
        where TDto : class, IBaseDto
        where TEntity : class, IBaseEntity
    {
        Task<TDto> GetByIdAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> AddAsync(TDto dto);
        Task<TDto> UpdateAsync(TDto dto);
        Task DeleteAsync(int id);
    }
}
