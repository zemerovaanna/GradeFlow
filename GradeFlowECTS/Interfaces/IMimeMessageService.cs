using MimeKit;

namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Сервис для создания MIME-сообщений электронной почты.
    /// </summary>
    public interface IMimeMessageService
    {
        /// <summary>
        /// Создает MIME-сообщение электронной почты с указанными параметрами.
        /// </summary>
        /// <param name="senderMail">Email адрес отправителя.</param>
        /// <param name="recipientEmail">Email адрес получателя.</param>
        /// <param name="content">Текст письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <returns>Созданное MIME-сообщение.</returns>
        MimeMessage CreateMailMessage(string senderMail, string recipientEmail, string content, string subject);
    }
}