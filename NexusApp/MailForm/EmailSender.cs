using MailKit.Security;
using MimeKit;
using NexusApp.Constants;
using NexusApp.ModelDTOs;
using System.Net.Mail;

namespace NexusApp.MailForm
{
    public class EmailSender
    {
        private readonly IRazorViewToStringRenderer _viewToStringRenderer;
        public EmailSender(IRazorViewToStringRenderer viewToStringRenderer)
        {
            _viewToStringRenderer = viewToStringRenderer;
        }

        public async Task SendEmailAsync(MailContextDTO mailContext, string viewContent)
        {
            /*string body = await _viewToStringRenderer.RenderViewToStringAsync("/Views/Emails/TemplateName.cshtml", mailContext);*/

            var message = new MimeMessage();
            message.Sender = new MailboxAddress(MailSending.DisplayName, MailSending.Email);
            message.From.Add(message.Sender);
            message.To.Add(new MailboxAddress(mailContext.Name, mailContext.To));
            message.Subject = mailContext.Subject;

            message.Body = new TextPart("html")
            {
                Text = viewContent
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                {
                    await smtp.ConnectAsync(MailSending.Host, MailSending.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(MailSending.Email, MailSending.Password);
                    await smtp.SendAsync(message);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
            smtp.Disconnect(true);
        }
    }
}
