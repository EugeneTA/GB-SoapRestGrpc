using Microsoft.EntityFrameworkCore;
using PetClinic.Data.Tables;

namespace PetClinic.Data
{
    public class PetClinicDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSession> AccountSessions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Appointment>()
                .HasOne(p => p.Pet)
                .WithMany(a => a.Appointments)
                .HasForeignKey(p => p.PetId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Appointment>()
                .HasOne(p => p.Client)
                .WithMany(a => a.Appointments)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.NoAction);
            
            //modelBuilder.Entity<AccountSession>()
            //    .Property(p => p.TimeCreated)
            //    .HasColumnType("datetime2")
            //    .HasPrecision(0)
            //    .IsRequired();

            //modelBuilder.Entity<AccountSession>()
            //    .Property(p => p.TimeClosed)
            //    .HasColumnType("datetime2")
            //    .HasPrecision(0)
            //    .IsRequired();

            //modelBuilder.Entity<AccountSession>()
            //    .Property(p => p.TimeLastRequest)
            //    .HasColumnType("datetime2")
            //    .HasPrecision(0)
            //    .IsRequired();
        }

        public PetClinicDbContext(DbContextOptions options) : base(options) { }
    }
}
