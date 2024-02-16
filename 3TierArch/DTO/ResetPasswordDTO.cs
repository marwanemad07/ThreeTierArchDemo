﻿namespace _3TierArch.DTO
{
    public class ResetPasswordDTO : PasswordConfirmation
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
