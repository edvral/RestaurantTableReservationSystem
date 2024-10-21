using Microsoft.EntityFrameworkCore;
using RestaurantTableReservationSystem.Models;

namespace RestaurantTableReservationSystem.Data
{
    public class RestaurantReservationContext : DbContext
    {
        public RestaurantReservationContext(DbContextOptions<RestaurantReservationContext> options)
        : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Tables)
                .WithOne(t => t.Restaurant)
                .HasForeignKey(t => t.RestaurantId);

            modelBuilder.Entity<Table>()
                .HasMany(t => t.Reservations)
                .WithOne(r => r.Table)
                .HasForeignKey(r => r.TableId);
        }
    }
}
