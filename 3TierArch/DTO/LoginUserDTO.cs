namespace _3TierArch.DTO
{
    public class LoginUserDTO
    {
        [Required]
        [MinLength(4)]
        public string email { get; set;}
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set;} 
    }
}
