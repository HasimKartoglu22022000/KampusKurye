using KampusKurye.Controllers;
using KampusKurye.Models;
using Microsoft.EntityFrameworkCore;

namespace KampusKurye.DbContexts
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<RestaurantModel> Restaurants { get; set; }
        public DbSet<CollegesModel> Colleges { get; set; }
        public DbSet<UsersModel> users { get; set; }
        public DbSet<ProductModel> products { get; set; }
        public DbSet<CategoriesModel> categories { get; set; }
        public DbSet<OrderModel> order { get; set; }
        public DbSet<OrderItemModel> order_items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RestaurantModel>()
                .HasOne(r => r.College)
                .WithMany(c => c.Restaurants)
                .HasForeignKey(r => r.college_id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
