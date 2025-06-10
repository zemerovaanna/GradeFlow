namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Сервис для работы с настройками электронной почты.
    /// Предоставляет методы для получения параметров SMTP (сервер, порт, почта отправителя, пароль) в расшифрованном виде.
    /// </summary>
    public interface IMailSettingsService
    {
        /// <summary>
        /// Получает адрес электронной почты отправителя в расшифрованном виде.
        /// </summary>
        /// <returns>Расшифрованный адрес электронной почты отправителя.</returns>
        string GetSenderMail();

        /// <summary>
        /// Получает пароль отправителя в расшифрованном виде.
        /// </summary>
        /// <returns>Расшифрованный пароль отправителя.</returns>
        string GetSenderPassword();

        /// <summary>
        /// Получает номер порта SMTP-сервера.
        /// </summary>
        /// <returns>Номер порта SMTP.</returns>
        string GetSmtpServer();

        /// <summary>
        /// Получает адрес SMTP-сервера в расшифрованном виде.
        /// </summary>
        /// <returns>Расшифрованный адрес SMTP-сервера.</returns>
        int GetSmtpPort();
    }
}