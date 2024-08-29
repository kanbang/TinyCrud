using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace Tiny.Services
{
    public interface IJsonFileService<T> where T : class, new()
    {
        Task<T> LoadAsync();
        Task SaveAsync(T data);
        Task PatchAsync(JsonPatchDocument<T> patchDoc);
        Task<TResult?> QueryAsync<TResult>(string jsonPath) where TResult : class;
        Task<JsonPatchDocument<T>> GenerateJsonPatchAsync(T newData);
    }
}
