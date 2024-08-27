using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Services
{
    public class JsonFileService<T> : IJsonFileService<T>
      where T : new()
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private T _data;

        public JsonFileService(string filePath)
        {
            _filePath = filePath;
        }

        // 异步加载 JSON 文件内容
        public async Task<T> LoadAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                {
                    _data = new T();
                    await SaveAsync(_data);  // 自动创建文件并初始化
                }
                else
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    _data = JsonConvert.DeserializeObject<T>(json) ?? new T();
                }
                return _data;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // 异步保存数据到 JSON 文件
        public async Task SaveAsync(T data)
        {
            await _semaphore.WaitAsync();
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                await File.WriteAllTextAsync(_filePath, json);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // 异步更新 JSON 文件中的特定数据
        public async Task PatchAsync(Action<T> updateAction)
        {
            await _semaphore.WaitAsync();
            try
            {
                // 确保最新数据被加载
                await LoadAsync();
                updateAction(_data);
                await SaveAsync(_data);  // 保存更新后的数据
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // 异步查询数据
        public async Task<TResult> QueryAsync<TResult>(Func<T, TResult> queryFunc)
        {
            await _semaphore.WaitAsync();
            try
            {
                // 确保最新数据被加载
                await LoadAsync();
                return queryFunc(_data);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}