using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Common
{
	public class EmailSettings
	{
		public string SmtpServer { get; set; }
		public int SmtpPort { get; set; }
		public string SmtpUsername { get; set; }
		public string SmtpPassword { get; set; }
		public string FromName { get; set; }
		public string FromAddress { get; set; }
		public bool EnableSsl { get; set; }
	}

	public class SendEmail {
		[Required(ErrorMessage = "Email address is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Subject is required")]
		public string Subject { get; set; }

		[Required(ErrorMessage = "Message is required")]
		public string Message { get; set; }
	}

	public class EmailResponse
	{
		public bool Success { get; set; }
		public string Message { get; set; }
	}

}


