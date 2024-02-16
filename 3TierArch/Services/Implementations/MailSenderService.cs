using _3TierArch.DTO;
using Microsoft.Extensions.Configuration;

namespace _3TierArch.Services.Implementations
{
    public class MailSenderService : IMailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly IMailSenderRepo _mailSender;

        public MailSenderService(IConfiguration configuration,
           IMailSenderRepo mailSender)
        {
            _configuration = configuration;
            _mailSender = mailSender;
        }
        public async Task<int> Send(MailDataDTO mail)
        {
            try {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration.GetSection("MailSettings:UserName").Value));
                email.To.Add(MailboxAddress.Parse(mail.EmailTo));
                email.Subject = mail.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = mail.Body };

                await _mailSender.Send(email);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return 0;
        }

    }
}
