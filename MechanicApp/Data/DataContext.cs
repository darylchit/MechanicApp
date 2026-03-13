using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MechanicApp.Models;

// Create after creating models

namespace MechanicApp.Data // 4. Installing Entity Framework Core and Setting Up the Data Context
{
    public class DataContext : IdentityDbContext<ApplicationUser> // 1 DB SETUP
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) // 2
        {
            //DataContext inherits from IdentityDbContext, this is a constructor that tells the database your configurations/settings for your app
        }

        // Note: ApplicationUser is managed by IdentityDbContext, no DbSet needed for it
        public DbSet<Client> Clients { get; set; } //DbSet tells DB what tables you have and what objects they are, allowing querying in C# // 3
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //many to many, how you go in and customize tables without going into DB
        {
            base.OnModelCreating(modelBuilder); // IMPORTANT: Call base for Identity tables (Users, Roles, etc.)

            // Client - ApplicationUser (One-to-One)
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne(u => u.Client)
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mechanic - ApplicationUser (One-to-One)
            modelBuilder.Entity<Mechanic>()
                .HasOne(m => m.User)
                .WithOne(u => u.Mechanic)
                .HasForeignKey<Mechanic>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ServiceRequest - Conversation (One-to-One optional)
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Conversation)
                .WithOne(c => c.ServiceRequest)
                .HasForeignKey<Conversation>(c => c.ServiceRequestId)
                .OnDelete(DeleteBehavior.SetNull);

            // Prevent cascade delete cycles - ServiceRequest -> Vehicle (use Restrict instead)
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Vehicle)
                .WithMany(v => v.ServiceRequests)
                .HasForeignKey(sr => sr.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete cycles - ServiceRequest -> Mechanic (use Restrict)
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Mechanic)
                .WithMany(m => m.ServiceRequests)
                .HasForeignKey(sr => sr.MechanicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete cycles - Conversation -> Client (use Restrict)
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Conversations)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete cycles - Conversation -> Mechanic (use Restrict)
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Mechanic)
                .WithMany(m => m.Conversations)
                .HasForeignKey(c => c.MechanicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal precision for costs
            modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.EstimatedCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.FinalCost)
                .HasPrecision(18, 2);
        }

    }
}