using JsonDiffPatchDotNet;
using JsonDiffPatchDotNet.Formatters.JsonPatch;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Services
{
    public class JsonFileService<T> : IJsonFileService<T> where T : class, new()
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private T _data;

        public JsonFileService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<T> LoadAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_data == null)
                {
                    _data = await LoadFromFileAsync();
                }
                return _data;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SaveAsync(T data)
        {
            await _semaphore.WaitAsync();
            try
            {
                await SaveToFileAsync(data);
                _data = data;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task PatchAsync(JsonPatchDocument<T> patchDoc)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_data == null)
                {
                    _data = await LoadFromFileAsync();
                }
                patchDoc.ApplyTo(_data);
                await SaveToFileAsync(_data);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<TResult?> QueryAsync<TResult>(string jsonPath) where TResult : class
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_data == null)
                {
                    _data = await LoadFromFileAsync();
                }
                return JObject.FromObject(_data).SelectToken(jsonPath)?.ToObject<TResult>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<JsonPatchDocument<T>> GenerateJsonPatchAsync(T newData)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_data == null)
                {
                    _data = await LoadFromFileAsync();
                }

                var jdp = new JsonDiffPatch();
                var originalJson = JObject.FromObject(_data);
                var modifiedJson = JObject.FromObject(newData);

                var patch = jdp.Diff(originalJson, modifiedJson);

                if (patch == null)
                {
                    return null;  // 如果没有差异，返回 null
                }

                // 使用 JsonDeltaFormatter 格式化差异为 JSON Patch 操作
                var formatter = new JsonDeltaFormatter();
                var operations = formatter.Format(patch);

                // 将 operations 序列化为 JSON 字符串
                var operationsJson = JsonConvert.SerializeObject(operations);

                // 反序列化为 JsonPatchDocument<T>
                var patchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<T>>(operationsJson);

                return patchDocument;

                // 将 JObject 转换为 JSON Patch 文档
                // var patchDocument = new JsonPatchDocument<T>(operations.ToString());
                // var operations = JsonConvert.DeserializeObject<List<Microsoft.AspNetCore.JsonPatch.Operations.Operation<T>>>(patch.ToString());
                // var patchStr = patch.ToString();
                // Console.Write(patchStr);
                // var operations = JsonConvert.DeserializeObject<List<Microsoft.AspNetCore.JsonPatch.Operations.Operation<T>>>(patchStr);

                // patchDocument.Operations.AddRange(operations);

                // return patchDocument;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<T> LoadFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                var newData = new T();
                await SaveToFileAsync(newData);  // 初始化文件
                return newData;
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<T>(json) ?? new T();
        }

        private async Task SaveToFileAsync(T data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }




    }
}
