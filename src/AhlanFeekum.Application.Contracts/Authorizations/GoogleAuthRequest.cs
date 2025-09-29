using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlanFeekum.Authorizations
{
    public class GoogleAuthRequest
    {
        public string IdToken { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
    }
}
