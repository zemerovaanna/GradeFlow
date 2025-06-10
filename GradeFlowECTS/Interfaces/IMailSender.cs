namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Реализует интерфейс IMailSender и отвечает за отправку электронных писем.
    /// </summary>
    public interface IMailSender
    {
        /// <summary>
        /// Отправляет письмо с верификационным кодом.
        /// </summary>
        /// <param name="recipientEmail">Email получателя.</param>
        /// <param name="verificationCode">Код верификации.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="_message">Дополнительный текст сообщения.</param>
        Task SendVerificationCode(string recipientEmail, string verificationCode, string subject, string messageTemplate);
    }
}