using System.Xml.Linq;

namespace CommonDTO
{
    public class EmailDTO
    {
        public string Email { get; set; }
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

    public class LotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = String.Empty;
        public double StartPrice { get; set; } = 1;
        public double CurrentPrice { get; set; } = 1;
    }
}
