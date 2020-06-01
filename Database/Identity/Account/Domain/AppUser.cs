using System;
using Microsoft.AspNetCore.Identity;

namespace Identity.Account
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string SocialSecurityNumber { get; set; }
    }
}