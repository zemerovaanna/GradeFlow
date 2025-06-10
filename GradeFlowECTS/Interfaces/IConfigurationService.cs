namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Сервис для работы с конфигурацией приложения.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Получает строку подключения к БД (расшифрованную).
        /// </summary>
        /// <returns>Строка подключения или значение по умолчанию.</returns>
        string GetConnectionString();

        /// <summary>
        /// Получает ключ шифрования.
        /// </summary>
        /// <returns>Ключ шифрования.</returns>
        string GetEncryptionKey();

        /// <summary>
        /// Записывает конфигурацию сервера в файл (в зашифрованном виде).
        /// </summary>
        /// <param name="server">Адрес сервера.</param>
        void WriteConfig(string server);
    }
}