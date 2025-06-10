using System.Diagnostics;
using System.IO;
using System.Text.Json;
using GradeFlowECTS.Data;
using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private const string FileName = "user_settings.json";
        private readonly string _filePath;
        private readonly IFileService _fileService;
        private UserSettings _userSettings;

        public UserSettingsService(IFileService fileService)
        {
            try
            {
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _filePath = _fileService.GetAppDataPath(FileName);
                _userSettings = LoadSettings() ?? new UserSettings { IsFirstLaunch = true };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UserSettingsService] Критическая ошибка при инициализации: {ex.Message}");
                return;
            }
        }

        public bool IsFirstLaunch
        {
            get => _userSettings.IsFirstLaunch;
            set
            {
                _userSettings.IsFirstLaunch = value;
                try
                {
                    Save();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[UserSettingsService] Ошибка при сохранении настроек: {ex.Message}");
                }
            }
        }

        private UserSettings? LoadSettings()
        {
            try
            {
                if (_fileService.Exists(_filePath))
                {
                    return _fileService.ReadJson<UserSettings>(_filePath);
                }
                Debug.WriteLine("[UserSettingsService] Файл настроек не найден, будут использованы настройки по умолчанию");
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"[UserSettingsService] Ошибка ввода-вывода при загрузке настроек: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"[UserSettingsService] Ошибка формата JSON при загрузке настроек: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UserSettingsService] Неизвестная ошибка при загрузке настроек: {ex.Message}");
            }

            return null;
        }

        public void ResetToDefault()
        {
            try
            {
                _userSettings = new UserSettings { IsFirstLaunch = true };
                Save();
                Debug.WriteLine("[UserSettingsService] Настройки сброшены к значениям по умолчанию");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UserSettingsService] Ошибка при сбросе настроек: {ex.Message}");
                return;
            }
        }

        private void Save()
        {
            try
            {
                string? directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    _fileService.CreateDirectory(directory);
                }
                _fileService.WriteJson(_filePath, _userSettings);
                Debug.WriteLine("[UserSettingsService] Настройки успешно сохранены");
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"[UserSettingsService] Ошибка ввода-вывода при сохранении: {ex.Message}");
                return;
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"[UserSettingsService] Ошибка сериализации JSON при сохранении: {ex.Message}");
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine($"[UserSettingsService] Ошибка доступа к файлу настроек: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UserSettingsService] Неизвестная ошибка при сохранении настроек: {ex.Message}");
                return;
            }
        }
    }
}