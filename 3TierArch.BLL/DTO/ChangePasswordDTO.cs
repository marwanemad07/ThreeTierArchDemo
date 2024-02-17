namespace _3TierArch.BLL.DTO
{
    public class ChangePasswordDTO : PasswordConfirmation
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = null!;
    }
}
