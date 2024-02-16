namespace _3TierArch.Repositories.Interfaces
{
    public interface IMailSenderRepo
    {
        public Task<int> Send(MimeMessage? email);
    }
}
