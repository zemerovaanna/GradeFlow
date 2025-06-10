using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GradeFlowECTS.Data;
using GradeFlowECTS.Models;


namespace GradeFlowECTS.Helpers
{
    /// <summary>
    /// Предоставляет функциональность для чтения и преобразования CSV-файла пользователей в объекты типа User.
    /// </summary>
    internal static class CsvParserService
    {
        /// <summary>
        /// Считывает CSV-файл по указанному пути, валидирует и преобразует записи в список пользователей.
        /// В случае ошибки возвращает пустой список.
        /// </summary>
        /// <param name="filePath">Путь к CSV-файлу.</param>
        /// <returns>Список объектов User.</returns>
        public static List<User> ParseUsersFromCsv(string filePath)
        {
            try
            {
                var config = GetCsvConfiguration();
                var csvModels = ReadCsvFile(filePath, config);
                return ConvertToUserModels(csvModels);
            }
            catch
            {
                return new List<User>();
            }
        }

        /// <summary>
        /// Создаёт конфигурацию для парсера CSV с заданными параметрами: разделитель, кодировка, отсутствие заголовков.
        /// </summary>
        /// <returns>Объект конфигурации CsvConfiguration.</returns>
        private static CsvConfiguration GetCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false,
                Encoding = Encoding.UTF8
            };
        }

        /// <summary>
        /// Считывает записи из CSV-файла и преобразует их в список моделей UserCsvModel.
        /// </summary>
        /// <param name="filePath">Путь к CSV-файлу.</param>
        /// <param name="config">Конфигурация CSV-парсера.</param>
        /// <returns>Список объектов UserCsvModel.</returns>
        private static List<UserCsvModel> ReadCsvFile(string filePath, CsvConfiguration config)
        {
            using var reader = new StreamReader(filePath, Encoding.UTF8, true);
            using var csv = new CsvReader(reader, config);

            return csv.GetRecords<UserCsvModel>().ToList();
        }

        /// <summary>
        /// Преобразует список моделей UserCsvModel в валидированные объекты типа User.
        /// Фильтрует записи с некорректным email-адресом.
        /// </summary>
        /// <param name="csvModels">Список моделей, считанных из CSV-файла.</param>
        /// <returns>Список валидных объектов User.</returns>
        private static List<User> ConvertToUserModels(IEnumerable<UserCsvModel> csvModels)
        {
            return csvModels
                .Where(x => InputValidator.IsValidMail(x.Mail, out var _))
                .Select(CreateUserFromCsvModel)
                .ToList();
        }

        /// <summary>
        /// Создаёт объект типа User на основе одной записи модели UserCsvModel.
        /// Генерирует уникальный идентификатор пользователя.
        /// </summary>
        /// <param name="csvModel">Модель пользователя из CSV.</param>
        /// <returns>Объект типа User.</returns>
        private static User CreateUserFromCsvModel(UserCsvModel csvModel)
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                LastName = csvModel.LastName,
                FirstName = csvModel.FirstName,
                MiddleName = csvModel.MiddleName,
                Mail = csvModel.Mail,
                RoleId = 1,
                Status = true
            };
        }
    }
}