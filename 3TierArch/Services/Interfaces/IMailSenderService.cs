using _3TierArch.DTO;

namespace _3TierArch.Services.Interfaces
{
    public interface IMailSenderService
    {
        public Task<int> Send(MailDataDTO email);
    }
}
