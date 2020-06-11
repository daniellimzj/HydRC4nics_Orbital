using System;
using Identity.Account.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    public class AppDatabaseContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
        }

        public new DbSet<AppUser> Users { get; set; }
    }
}