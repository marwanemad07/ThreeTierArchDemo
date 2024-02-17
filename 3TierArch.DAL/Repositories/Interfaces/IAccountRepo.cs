﻿namespace _3TierArch.DAL.Repositories.Interfaces
{
    public interface IAccountRepo
    {
        public Task<IdentityResult> CreateUser(ApplicationUser newUser, string password);
        public Task<string> GetConfirmationToken(ApplicationUser newUser);
        public Task<string> GetResetPasswordToken(ApplicationUser user);
        public Task<bool> ConfirmEmail(ApplicationUser user, string token);
        public Task<ApplicationUser?> GetUserById(string userId);
        public Task<ApplicationUser?> GetUserByName(string name);
        public Task<ApplicationUser?> GetUserByEmail(string email);
        public Task<bool> CheckPassword(ApplicationUser user, string password);
        public Task<IList<string>?> GetRoles(ApplicationUser user);
        public Task<bool> ResetPassword(ApplicationUser user, string token, string password);
        public Task<bool> ChangePassword(ApplicationUser user, string oldPassword, string newPassword);
    }
}
