using EFCoreSample.Libraries.Data;
using EFCoreSample.Movies.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options)
        { }

        public DbSet<LibraryDataModel> Libraries { get; set; }
        public DbSet<BookDataModel> Books { get; set; }
        public DbSet<MovieDataModel> Movies { get; set; }
        public DbSet<RentalDataModel> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configures the relationship such that a LibraryDataModel has many BookDataModels, with all BookDataModels
            // requiring a LibraryDataModel
            modelBuilder.Entity<LibraryDataModel>()
                .HasMany(e => e.Books)
                .WithOne()
                .IsRequired();
                
            modelBuilder.Entity<RentalDataModel>()
                .HasMany(e => e.Movies)
                .WithOne()
                .IsRequired();
        }
    }
}