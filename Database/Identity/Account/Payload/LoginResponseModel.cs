using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.Account.Payload
{
    public class LoginResponseModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}