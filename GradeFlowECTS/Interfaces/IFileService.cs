namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Предоставляет сервис для работы с файлами данных приложения,
    /// сохраняемыми в стандартной папке (%LOCALAPPDATA%/GradeFlow).
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Создает все каталоги и подкаталоги в указанном пути, если они еще не существуют.
        /// </summary>
        void CreateDirectory(string directoryPath);

        /// <summary>
        /// Проверяет существование файла по указанному полному пути.
        /// </summary>
        bool Exists(string filePath);

        /// <summary>
        /// Получает путь к файлу или директории внутри папки данных приложения (%LOCALAPPDATA%/GradeFlow).
        /// Гарантирует, что базовая директория приложения существует.
        /// </summary>
        /// <param name="relativeFilePath">Относительный путь к файлу или папке внутри директории данных приложения (например, "settings.json" или "data/cache"). Если пусто, возвращает путь к самой директории данных.</param>
        /// <returns>Полный путь к файлу или директории данных приложения.</returns>
        string GetAppDataPath(string relativeFilePath = "");

        /// <summary>
        /// Читает все содержимое файла как текст (синхронно).
        /// </summary>
        string ReadAllText(string filePath);

        /// <summary>
        /// Асинхронно читает все содержимое файла как текст.
        /// </summary>
        Task<string> ReadAllTextAsync(string filePath);

        /// <summary>
        /// Читает файл и десериализует его содержимое как JSON указанного типа.
        /// Выбрасывает исключение, если файл не найден или содержит некорректный JSON.
        /// </summary>
        T ReadJson<T>(string filePath);

        /// <summary>
        /// Асинхронно читает файл и десериализует его содержимое как JSON указанного типа.
        /// Выбрасывает исключение, если файл не найден или содержит некорректный JSON.
        /// </summary>
        Task<T> ReadJsonAsync<T>(string filePath);

        /// <summary>
        /// Записывает текст в файл (синхронно, перезаписывает если существует).
        /// Гарантирует создание директории перед записью.
        /// </summary>
        void WriteAllText(string filePath, string content);

        /// <summary>
        /// Асинхронно записывает текст в файл.
        /// Гарантирует создание директории перед записью.
        /// </summary>
        Task WriteAllTextAsync(string filePath, string content);

        /// <summary>
        /// Сериализует объект в JSON и записывает в файл.
        /// Гарантирует создание директории перед записью.
        /// </summary>
        /// <param name="formatJson">Форматировать ли JSON с отступами (true) или нет (false).</param>
        void WriteJson<T>(string filePath, T obj, bool formatJson = true);

        /// <summary>
        /// Асинхронно сериализует объект в JSON и записывает в файл.
        /// Гарантирует создание директории перед записью.
        /// </summary>
        /// <param name="formatJson">Форматировать ли JSON с отступами (true) или нет (false).</param>
        Task WriteJsonAsync<T>(string filePath, T obj, bool formatJson = true);
    }
}