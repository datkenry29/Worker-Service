using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SendMailService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse("email de gui");
                email.To.Add(MailboxAddress.Parse("email nhan"));
                email.Subject = "Worker Service Send Mail";
                var builder = new BodyBuilder();
                builder.HtmlBody = "Worker Service Hello";
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 25, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("email de gui", "password cua email gui");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(600000, stoppingToken);
            }
        }
    }
}