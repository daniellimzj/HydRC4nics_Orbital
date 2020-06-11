using System;
using Microsoft.AspNetCore.Identity;

namespace Identity.Account.Domain
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
    }
}