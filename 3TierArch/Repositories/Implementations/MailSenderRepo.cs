namespace _3TierArch.Repositories.Implementations
{
    public class MailSenderRepo : IMailSenderRepo
    {
        private readonly IConfiguration _configuration;
        public MailSenderRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<int> Send(MimeMessage? email)
        {
            try {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_configuration.GetSection("MailSettings:Host").Value,
                    int.Parse(_configuration.GetSection("MailSettings:Port").Value),
                    SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_configuration.GetSection("MailSettings:UserName").Value,
                    _configuration.GetSection("MailSettings:Password").Value);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return 0;
        }
    }
}
