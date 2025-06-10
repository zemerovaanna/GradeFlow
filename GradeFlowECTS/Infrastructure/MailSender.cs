using GradeFlowECTS.Interfaces;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using System.Diagnostics;

namespace GradeFlowECTS.Infrastructure
{
    public class MailSender : IMailSender
    {
        private readonly IMailSettingsService _mailSettingsService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IMimeMessageService _mimeMessageService;

        public MailSender(IMailSettingsService mailSettingsService, IMessageBoxService messageBoxService, IMimeMessageService mimeMessageService)
        {
            _mailSettingsService = mailSettingsService ?? throw new ArgumentNullException(nameof(mailSettingsService));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _mimeMessageService = mimeMessageService ?? throw new ArgumentNullException(nameof(mimeMessageService));
        }

        /// <summary>
        /// Асинхронно отправляет электронное письмо через SMTP
        /// </summary>
        /// <param name="message">MIME-сообщение для отправки</param>
        /// <remarks>
        /// Подключается к SMTP-серверу с TLS-шифрованием,
        /// аутентифицируется и отправляет сообщение.
        /// Показывает уведомление об успехе или ошибке.
        /// </remarks>
        private async Task SendMailAsync(MimeMessage message)
        {
            const string methodName = "[SendMailAsync]";

            try
            {
                using (var client = new SmtpClient())
                {
                    try
                    {
                        var smtpServer = _mailSettingsService.GetSmtpServer();
                        var smtpPort = _mailSettingsService.GetSmtpPort();

                        if (string.IsNullOrEmpty(smtpServer))
                        {
                            Debug.WriteLine($"{methodName} Ошибка: SMTP сервер не указан");
                            _messageBoxService.ShowError("Ошибка конфигурации почтового сервера");
                            return;
                        }

                        Debug.WriteLine($"{methodName} Подключение к SMTP серверу {smtpServer}:{smtpPort}");
                        await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);

                        var senderMail = _mailSettingsService.GetSenderMail();
                        var senderPassword = _mailSettingsService.GetSenderPassword();

                        if (string.IsNullOrEmpty(senderMail) || string.IsNullOrEmpty(senderPassword))
                        {
                            Debug.WriteLine($"{methodName} Ошибка: Не указаны учетные данные для SMTP");
                            _messageBoxService.ShowError("Ошибка конфигурации почтового сервера");
                            return;
                        }

                        Debug.WriteLine($"{methodName} Аутентификация пользователя {senderMail}");
                        await client.AuthenticateAsync(senderMail, senderPassword);

                        Debug.WriteLine($"{methodName} Отправка сообщения");
                        await client.SendAsync(message);

                        await client.DisconnectAsync(true);

                        Debug.WriteLine($"{methodName} Сообщение успешно отправлено");
                        _messageBoxService.ShowInformation(
                            "Сообщение отправлено на вашу почту. Если сообщения нет, попробуйте проверить папку Спам.",
                            "Уведомление");
                    }
                    catch (SmtpCommandException ex)
                    {
                        Debug.WriteLine($"{methodName} Ошибка SMTP: {ex.StatusCode} - {ex.Message}");
                        _messageBoxService.ShowError($"Ошибка отправки письма: {ex.Message}");
                    }
                    catch (SmtpProtocolException ex)
                    {
                        Debug.WriteLine($"{methodName} Протокольная ошибка SMTP: {ex.Message}");
                        _messageBoxService.ShowError("Ошибка протокола при отправке письма");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{methodName} Критическая ошибка: {ex}");
                _messageBoxService.ShowError("Не удалось отправить электронное письмо");
            }
        }

        public async Task SendVerificationCode(string recipientEmail, string verificationCode, string subject, string message)
        {
            const string methodName = "[SendVerificationCode]";

            try
            {
                if (string.IsNullOrEmpty(recipientEmail))
                {
                    Debug.WriteLine($"{methodName} Ошибка: Не указан получатель");
                    _messageBoxService.ShowError("Не указан адрес получателя");
                    return;
                }

                var senderMail = _mailSettingsService.GetSenderMail();
                if (string.IsNullOrEmpty(senderMail))
                {
                    Debug.WriteLine($"{methodName} Ошибка: Не указан отправитель");
                    _messageBoxService.ShowError("Ошибка конфигурации почтового сервера");
                    return;
                }

                Debug.WriteLine($"{methodName} Создание письма для {recipientEmail}");
                var mailMessage = _mimeMessageService.CreateMailMessage(
                    senderMail,
                    recipientEmail,
                    $"{message}{verificationCode}",
                    subject);

                Debug.WriteLine($"{methodName} {senderMail}, {recipientEmail},{message}: {verificationCode}, {subject}");

                await SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{methodName} Ошибка: {ex.Message}");
                _messageBoxService.ShowError("Ошибка при подготовке письма с кодом подтверждения");
            }
        }
    }
}