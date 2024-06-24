namespace CommonDTO
{
    public class RegisterUserRequest
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
    public class AuthUserRequest
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
