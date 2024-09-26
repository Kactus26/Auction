namespace CommonDTO
{

    public class ChangeUserPasswordDTO
    {
        public required int Id { get; set; }
        public string Password { get; set; } = null!;
    }

    public class LoginDTO//You can't just send string
    {
        public string Login { get; set; } = null!;
    }

    public class UserIdDTO//You can't just send int
    {
        public required int Id { get; set; }
    }

    public class EmailDTO//You can't just send string
    {
        public string Email { get; set; } = null!;
    }

    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = null!;
        public bool IsEmailConfirmed { get; set; } = false;
        public string Info { get; set; } = String.Empty;
        public string? ImageUrl { get; set; }
        public double Balance { get; set; } = 0.00;
    }

    public class UserDataWithImageDTO
    {
        public UserProfileDTO ProfileData { get; set; } = null!;
        public byte[]? Image { get; set; }
    }

    
}
