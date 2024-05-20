using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class RegisterUserRequest
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}
