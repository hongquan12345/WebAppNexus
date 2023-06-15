using MailKit.Security;
using MimeKit;
using NexusApp.Constants;
using NexusApp.ModelDTOs;

public class MailUtils
{
    public MailUtils()
    {

    }
    public async Task SendMail(MailContextDTO mailContext)
    {
        var email = new MimeMessage();
        email.Sender = new MailboxAddress(MailSending.DisplayName, MailSending.Email);
        email.From.Add(email.Sender);
        email.To.Add(new MailboxAddress(mailContext.Name, mailContext.To));
        email.Subject = mailContext.Subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = mailContext.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        try
        {
            {
                await smtp.ConnectAsync(MailSending.Host, MailSending.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(MailSending.Email, MailSending.Password);
                await smtp.SendAsync(email);
            }
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);

        }
        smtp.Disconnect(true);
    }
}

