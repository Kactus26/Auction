using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient.Data
{
    internal class LoggedUser
    {
        public int Id { get; set; }
        public string JWTToken { get; set; }
    }
}
