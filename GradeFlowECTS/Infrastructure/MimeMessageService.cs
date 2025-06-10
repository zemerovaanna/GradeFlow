using GradeFlowECTS.Interfaces;
using MimeKit;

namespace GradeFlowECTS.Infrastructure
{
    public class MimeMessageService : IMimeMessageService
    {
        public MimeMessage CreateMailMessage(string senderMail, string recipientEmail, string content, string subject)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support GradeFlow", senderMail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = content };

            return message;
        }
    }
}