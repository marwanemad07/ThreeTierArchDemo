namespace _3TierArch.BLL.Services.Interfaces
{
    public interface IMailSenderService
    {
        public Task<int> Send(MailDataDTO email);
    }
}
