using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
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
}
