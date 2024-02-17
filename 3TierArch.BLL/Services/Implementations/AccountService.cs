using System.Security.Claims;
using System.Text;
using System.Web;

namespace _3TierArch.BLL.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly IMailSenderService _mailSender;
        private readonly IConfiguration _configuration;

        public AccountService(IAccountRepo accountRepo,
            IMailSenderService mailSender,
            IConfiguration configuration)
        {
            _accountRepo = accountRepo;
            _mailSender = mailSender;
            _configuration = configuration;
        }

        public async Task<int> Register(RegisterUserDTO user, string confirmEmailUrl)
        {
            var newUser = new ApplicationUser() {
                Email = user.Email,
                UserName = user.Username
            };
            var isCreated = await _accountRepo.CreateUser(newUser, user.Password);

            if (isCreated.Succeeded) {
                var code = await _accountRepo.GetConfirmationToken(newUser);
                var mailData = GetMailData(newUser, confirmEmailUrl, code,
                    "Please confirm your email here", "Confirm Email");
                return await _mailSender.Send(mailData);
            }
            Console.WriteLine(isCreated.Errors.First());
            return -1;
        }
        public async Task<int> ConfirmEmail(string userId, string code)
        {
            var user = await _accountRepo.GetUserById(userId);
            if (user == null || user.EmailConfirmed)
                return -1;

            var result = await _accountRepo.ConfirmEmail(user, code);
            return result ? 0 : -1;
        }
        public async Task<JwtSecurityToken?> Login(LoginUserDTO user)
        {
            var existUser = await _accountRepo.GetUserByEmail(user.Email);
            if (IsNotConfirmedUser(existUser))
                return null;

            var isAuthenticated = await _accountRepo.CheckPassword(existUser!, user.Password);
            if (!isAuthenticated)
                return null;

            // Create JWT token
            var token = await CreateToken(existUser!);
            return token;
        }
        public async Task<int> ForgotPassword(string email, string confirmEmailUrl)
        {
            var user = await _accountRepo.GetUserByEmail(email);
            if (IsNotConfirmedUser(user))
                return -1;

            var code = await _accountRepo.GetResetPasswordToken(user!); 
            var mailData = GetMailData(user!, confirmEmailUrl, code,
                "Please reset you password here", "Reset Password");
            return await _mailSender.Send(mailData);
        }
        public async Task<int> ResetPassword(ResetPasswordDTO request)
        {
            var user = await _accountRepo.GetUserById(request.UserId);
            if (user == null)
                return -1;
            request.Token = HttpUtility.UrlDecode(request.Token);
            var result = await _accountRepo.ResetPassword(user, request.Token, request.Password);
            return result ? 0 : -1;
        }
        public async Task<int> ChangePassword(ChangePasswordDTO request)
        {
            var user = await _accountRepo.GetUserById(request.UserId);
            if (user == null)
                return -1;
            var result = await _accountRepo.ChangePassword(user!, request.OldPassword, request.Password);
            return result ? 0 : -1;
        }
        private MailDataDTO GetMailData(ApplicationUser user, string url, string code,
            string message, string subject)
        {
            var callBackUrl = $"{url}?userId={user.Id}&code={HttpUtility.UrlEncode(code)}";

            var mailData = new MailDataDTO() {
                EmailTo = user.Email!,
                Subject = subject,
                Body = $"{message} <a href=\"{callBackUrl}\">Clikc here</a>"
            };
            return mailData;
        }
        private async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim> {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName!),
                new (ClaimTypes.Email, user.Email!),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _accountRepo.GetRoles(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
                );

            return token;
        }
        private bool IsNotConfirmedUser(ApplicationUser? user)
        {
            return user == null || !user.EmailConfirmed;
        }
    }
}
