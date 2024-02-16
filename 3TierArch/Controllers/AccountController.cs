namespace _3TierArch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;

        public AccountController(IConfiguration configuration,
            IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO newUser)
        {
            if (ModelState.IsValid) {
                var confirmEmailUrl = GetCallBackUrl(nameof(ConfirmEmail));
                var result = await _accountService.Register(newUser, confirmEmailUrl);
                if (result == 0)
                    return Ok("Check your mail for confirmation url");
                else
                    return BadRequest("Something went wrong at registeration"); // may be Ok and tell him to request email verification
            }
            return BadRequest(ModelState.First());
        }
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (ModelState.IsValid) {
                if (userId != null && code != null) {
                    var result = await _accountService.ConfirmEmail(userId, code);
                    if (result == 0)
                        return NoContent();
                    else
                        return BadRequest("Wrong code or user doesn't exist");
                }
                return BadRequest("Invalid data");
            }
            return BadRequest(ModelState.First());
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO user)
        {
            if (ModelState.IsValid) {
                var token = await _accountService.Login(user);
                if (token != null)
                    return Ok(new {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    }); // should return JWT token here
                return BadRequest("Something went wrong");
            }
            return BadRequest(ModelState.First());
        }
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([DataType(DataType.EmailAddress)]string email)
        {
            if (ModelState.IsValid) {
                var confirmEmailUrl = GetCallBackUrl(nameof(ResetPassword));
                var result = await _accountService.ForgotPassword(email, confirmEmailUrl);
                
                if(result == 0)
                    return Ok("Check your mail for reset password link");
                else
                    return BadRequest("Something went wrong at forgot password");
            }
            return BadRequest(ModelState.First());
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO request)
        {
            if (ModelState.IsValid) {
                var result = await _accountService.ResetPassword(request);
                if(result == 0)
                    return NoContent();
                return BadRequest("Something went wrong at reset password");
            }
            return BadRequest(ModelState.First());
        }
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO request)
        {
            if (ModelState.IsValid) {
                var result = await _accountService.ChangePassword(request);
                if(result == 0)
                    return NoContent();
                return BadRequest("Something went wrong at change password");
            }
            return BadRequest(ModelState.First());
        }
        private string GetCallBackUrl(string action) => $"{Request.Scheme}://{Request.Host}{Url.Action(action, nameof(AccountController).Replace("Controller", ""))}";
    }
}