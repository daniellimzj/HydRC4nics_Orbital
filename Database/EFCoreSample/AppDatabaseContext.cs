using EFCoreSample.Controls.Data;
using EFCoreSample.Monitoring.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options)
        { }

        public DbSet<SensorDataModel> Sensors { get; set; }
        public DbSet<ReadingDataModel> Readings { get; set; }
        public DbSet<ActuatorDataModel> Actuators { get; set; }
        public DbSet<CommandDataModel> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configures the relationship such that a LibraryDataModel has many BookDataModels, with all BookDataModels
            // requiring a LibraryDataModel
            modelBuilder.Entity<SensorDataModel>()
                .HasMany(sensor => sensor.Readings)
                .WithOne()
                .IsRequired();
                
            modelBuilder.Entity<ActuatorDataModel>()
                .HasMany(actuator => actuator.Commands)
                .WithOne()
                .IsRequired();
        }
    }
}