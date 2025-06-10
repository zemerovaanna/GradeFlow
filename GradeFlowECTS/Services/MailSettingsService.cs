using GradeFlowECTS.Data;
using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.Services
{
    internal class MailSettingsService : IMailSettingsService
    {
        private readonly IConfigurationService _configurationService;
        private readonly ICryptographyService _cryptographyService;
        private static MailSettings _mailSettings;

        public MailSettingsService(IConfigurationService configurationService, ICryptographyService cryptographyService)
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
            _mailSettings = new MailSettings();
        }

        public string GetSenderMail() => _cryptographyService.Decrypt(_mailSettings.SenderMail, _configurationService.GetEncryptionKey());

        public string GetSenderPassword() => _cryptographyService.Decrypt(_mailSettings.SenderPassword, _configurationService.GetEncryptionKey());

        public int GetSmtpPort() => _mailSettings.SmtpPort;

        public string GetSmtpServer() => _cryptographyService.Decrypt(_mailSettings.SmtpServer, _configurationService.GetEncryptionKey());
    }
}