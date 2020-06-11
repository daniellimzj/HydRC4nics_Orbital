using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.Account.Payload
{
    public class AccountResponseModel
    {
        public string Guid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}