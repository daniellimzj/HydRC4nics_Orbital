using System;
using Identity.Account;
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

        public DbSet<AppUser> Users { get; set; }
    }
}