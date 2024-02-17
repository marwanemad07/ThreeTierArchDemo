namespace _3TierArch.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<int> Register(RegisterUserDTO user, string confirmEmailUrl);
        public Task<int> ConfirmEmail(string userId, string code);
        public Task<JwtSecurityToken?> Login(LoginUserDTO user);
        public Task<int> ForgotPassword(string email, string confirmEmailUrl);
        public Task<int> ResetPassword(ResetPasswordDTO request);
        public Task<int> ChangePassword(ChangePasswordDTO request);
    }
}
