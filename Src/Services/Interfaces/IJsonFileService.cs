
namespace Tiny.Services
{
    public interface IJsonFileService<T>
    {
        Task<T> LoadAsync();
        Task SaveAsync(T data);
        Task PatchAsync(Action<T> updateAction);
        Task<TResult> QueryAsync<TResult>(Func<T, TResult> queryFunc);
    }
}