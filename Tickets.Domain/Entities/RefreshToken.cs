using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Domain.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public string? RefreshTokenValue { get; set; }
        public bool Active { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime Created { get; set; }
        public bool Used { get; set; }
        public string? UserId { get; set; }
        public string? Token { get; set; }
    }
}
