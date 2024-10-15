using Application.Features.DTOs;
using Application.Features.Interfaces.IService;
using MimeKit;


namespace Application.Features.Queries.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly string smtpServer = "sandbox.smtp.mailtrap.io";
        private readonly int smtpPort = 465;
        string username = "c3a70fdd62b5ea";
        string password = "fb2c6b9b9faefe";
        string senderEmail = "no-reply@AIQuiz.com";
        public async Task SendEmail(EmailRequestModel mailRequest)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("AIQuiz", senderEmail));
            message.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            message.Subject = mailRequest.Subject;

            var body = new TextPart("html")
            {
                Text = mailRequest.HtmlContent,
            };
            message.Body = body;

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await client.ConnectAsync(smtpServer, smtpPort, false);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

        public async Task SendWelcomeMessage(string email, string name, string code)
        {
            
            string subject = "Welcome To AIQuiz";
            string html = $@"
    <body>
        <div>
            <h1>Welcome to AIQuiz!</h1>
            <p3>Hi <strong>{name}</strong>,</p3>
            <p>Welcome to AIExaminer, where your document becomes a tool for learning! We're excited to have you on board. With AIExaminer, you can easily upload any document and generate custom quiz questions instantly.</p>
            <p>Ready to get started? Simply upload your document, and we'll do the rest!</p>
            <a href=""{code}"">Confirm your mail first :)</a>
        </div>
        <div>
            <p>If you have any questions or need assistance, feel free to <a href=""mailto:support@aiexaminer.com"">contact our support team</a>.</p>
            <p>Thank you for choosing AIExaminer!</p>
        </div>
    </body>";

            var model = new EmailRequestModel
            {
                ToName = name,
                HtmlContent = html,
                Subject = subject,
                ToEmail = email,
            };
            await SendEmail(model);
        }

        public async Task SubscriptionMessage()
        {

        }

    }
}
