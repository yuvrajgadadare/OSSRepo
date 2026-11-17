
using OmSaiModels.Common;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;



namespace OmSaiServices.Common
{


	public interface IEmailService
	{
		Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
	}

	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;

		public EmailService(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings), "Email settings cannot be null.");
		}

		public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
		{
			if (string.IsNullOrWhiteSpace(toEmail))
				throw new ArgumentException("Recipient email cannot be null or empty.", nameof(toEmail));

			if (string.IsNullOrWhiteSpace(_emailSettings.FromAddress))
				throw new InvalidOperationException("Sender email (FromAddress) is not configured.");

			var email = new MimeMessage();
			email.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
			email.To.Add(new MailboxAddress("", toEmail));
			email.Subject = subject;

			var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
			email.Body = bodyBuilder.ToMessageBody();

			using var client = new SmtpClient();
			await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, _emailSettings.EnableSsl ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.StartTls);
			await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
			await client.SendAsync(email);
			await client.DisconnectAsync(true);
		}
	}



}
