using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlanFeekum.Authorizations
{
    public class TokenRequest
    {
        public string PhoneOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
