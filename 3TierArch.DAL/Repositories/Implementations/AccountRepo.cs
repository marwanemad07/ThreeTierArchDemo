namespace _3TierArch.DAL.Repositories.Implementations
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRepo(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityResult> CreateUser(ApplicationUser newUser, string password)
        {
            return _userManager.CreateAsync(newUser, password);
        }

        public async Task<string> GetConfirmationToken(ApplicationUser newUser)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        }
        public async Task<string> GetResetPasswordToken(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<bool> ConfirmEmail(ApplicationUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
        public async Task<ApplicationUser?> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser?> GetUserByName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<bool> CheckPassword(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<IList<string>?> GetRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<bool> ResetPassword(ApplicationUser user, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }
        public async Task<bool> ChangePassword(ApplicationUser user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

    }
}
