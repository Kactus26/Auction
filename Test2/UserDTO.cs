using System.Xml.Linq;

namespace CommonDTO
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = null!;
        public string Info { get; set; } = String.Empty;
        public string? ImageUrl { get; set; }
        public double Balance { get; set; } = 0.00;
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
