using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cinematicks.Services
{
    public class MyEmailService : IEmailSender
    {
		private readonly IConfiguration conf;
		private SmtpClient MailServer { get; set; }
		public MyEmailService(IConfiguration _configuration)
		{
			conf = _configuration;
			MailServer = new SmtpClient
			{
				Host = conf.GetSection("EmailService")["SMTPServer"],
				Port = Convert.ToInt32(conf.GetSection("EmailService")["SMTPPort"]),
				EnableSsl = true,
				Credentials = new NetworkCredential(conf.GetSection("EmailService")["Username"], conf.GetSection("EmailService")["Password"])
			};
		}

		public Task SendEmailAsync(string email, string subject, string message)
		{
			MailAddress emailFrom = new MailAddress(conf.GetSection("EmailService")["Username"], conf.GetSection("EmailService")["From"]);
			MailAddress emailTo = new MailAddress(email);
			MailMessage emailMsg = new MailMessage(emailFrom, emailTo)
			{
				Subject = subject,
				Body = message,
				IsBodyHtml = true
			};
			MailServer.Send(emailMsg);
			return Task.CompletedTask;
		}
	}
}
