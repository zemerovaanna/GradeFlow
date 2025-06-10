using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.Infrastructure
{
    public class FileService : IFileService, IDisposable
    {
        private readonly string _appName;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

        public FileService(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
                throw new ArgumentException("Имя приложения не может быть пустым", nameof(appName));

            _appName = appName;

            _jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = null,
                WriteIndented = true
            };

            string basePath = GetAppDataPath();
            CreateDirectory(basePath);
        }

        public void CreateDirectory(string directoryPath)
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or PathTooLongException or IOException)
            {
                throw new InvalidOperationException($"Не удалось создать директорию: {directoryPath}", ex);
            }
        }

        public void Dispose()
        {
            _fileLock?.Dispose();
        }

        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string GetAppDataPath(string relativeFilePath = "")
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string basePath = Path.Combine(localAppData, _appName);

            return string.IsNullOrEmpty(relativeFilePath)
                ? basePath
                : Path.Combine(basePath, relativeFilePath);
        }

        public string ReadAllText(string filePath)
        {
            if (!Exists(filePath))
                throw new FileNotFoundException("Файл не найден", filePath);

            try
            {
                _fileLock.Wait();
                return File.ReadAllText(filePath, Encoding.UTF8);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task<string> ReadAllTextAsync(string filePath)
        {
            if (!Exists(filePath))
                throw new FileNotFoundException("Файл не найден", filePath);

            try
            {
                await _fileLock.WaitAsync();
                using StreamReader reader = new StreamReader(filePath, Encoding.UTF8);
                return await reader.ReadToEndAsync();
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public T ReadJson<T>(string filePath)
        {
            string json = ReadAllText(filePath);
            try
            {
                return JsonSerializer.Deserialize<T>(json, _jsonOptions) ??
                       throw new InvalidOperationException("Deserialization returned null");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Не удалось десериализовать JSON из файла: {filePath}", ex);
            }
        }

        public async Task<T> ReadJsonAsync<T>(string filePath)
        {
            string json = await ReadAllTextAsync(filePath);
            try
            {
                return JsonSerializer.Deserialize<T>(json, _jsonOptions) ??
                       throw new InvalidOperationException("Deserialization returned null");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Не удалось десериализовать JSON из файла: {filePath}", ex);
            }
        }

        public void WriteAllText(string filePath, string content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            try
            {
                _fileLock.Wait();
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    CreateDirectory(directory);
                }

                File.WriteAllText(filePath, content, Encoding.UTF8);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task WriteAllTextAsync(string filePath, string content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            try
            {
                await _fileLock.WaitAsync();
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    CreateDirectory(directory);
                }

                await using StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8);
                await writer.WriteAsync(content);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public void WriteJson<T>(string filePath, T obj, bool formatJson = true)
        {
            JsonSerializerOptions options = new JsonSerializerOptions(_jsonOptions)
            {
                WriteIndented = formatJson
            };

            string json = JsonSerializer.Serialize(obj, options);
            WriteAllText(filePath, json);
        }

        public async Task WriteJsonAsync<T>(string filePath, T obj, bool formatJson = true)
        {
            JsonSerializerOptions options = new JsonSerializerOptions(_jsonOptions)
            {
                WriteIndented = formatJson
            };

            string json = JsonSerializer.Serialize(obj, options);
            await WriteAllTextAsync(filePath, json);
        }
    }
}