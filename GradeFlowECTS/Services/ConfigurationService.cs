using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private const string DefaultString = "DefaultString";
        private const string FileName = "gradeflow_config.txt";

        private readonly ICryptographyService _cryptographyService;
        private readonly IFileService _fileService;
        private readonly IUserSettingsService _userSettingsService;

        public ConfigurationService(ICryptographyService cryptoService, IFileService fileService, IUserSettingsService userSettingsService)
        {
            _cryptographyService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _userSettingsService = userSettingsService ?? throw new ArgumentNullException(nameof(userSettingsService));
        }

        public string GetConnectionString()
        {
            string content = GetConfigFileData();
            return string.IsNullOrEmpty(content) ? DefaultString : _cryptographyService.Decrypt(content, GetEncryptionKey());
        }

        public string GetEncryptionKey() => "GradeFlowWPF";

        /// <summary>
        /// Читает данные из конфигурационного файла.
        /// </summary>
        /// <returns>Содержимое файла или пустая строка.</returns>
        private string GetConfigFileData()
        {
            string filePath = GetConfigFilePath();

            if (!_fileService.Exists(filePath))
            {
                _userSettingsService.IsFirstLaunch = true;
                return string.Empty;
            }
            else
            {
                return _fileService.ReadAllText(filePath);
            }
        }

        /// <summary>
        /// Получает путь к конфигурационному файлу.
        /// </summary>
        /// <returns>Полный путь к файлу конфигурации.</returns>
        private string GetConfigFilePath() => _fileService.GetAppDataPath(FileName);

        public void WriteConfig(string server)
        {
            string filePath = GetConfigFilePath();
            string encrypted = _cryptographyService.Encrypt($"Server={server};Database=GradeFlow;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;", GetEncryptionKey());

            _fileService.WriteAllText(filePath, encrypted);
        }
    }
}