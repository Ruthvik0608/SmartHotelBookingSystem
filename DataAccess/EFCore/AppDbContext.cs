using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartHotelBookingSystem.Models;


namespace SmartHotelBookingSystem.DataAccess.EFCore
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet properties for your entities
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<LoyaltyAccount> LoyaltyAccounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Redemption> Redemptions { get; set; }
        public DbSet<Review> Reviews { get; set; }

        

        // Override OnModelCreating if needed

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>().HasKey(q => q.UserID);
            modelBuilder.Entity<Booking>().HasKey(q => q.BookingID);
            modelBuilder.Entity<Review>().HasKey(q => q.ReviewID);
            modelBuilder.Entity<LoyaltyAccount>().HasKey(q => q.LoyaltyID);
            modelBuilder.Entity<CategoryModel>().HasKey(q => q.CatId);
            

            // Additional configuration
        }
    }
}
