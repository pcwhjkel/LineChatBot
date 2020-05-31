using Microsoft.EntityFrameworkCore;

public class BfStoreDbContext : DbContext {
    public BfStoreDbContext (DbContextOptions<BfStoreDbContext> options) : base (options) {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // modelBuilder.Entity<DrinkSet>()
        //             .HasKey(m => new { m.UserId, m.OrderDate });

        // modelBuilder.Entity<FoodSet>()
        //             .HasKey(m => new { m.UserId, m.OrderDate, m.ItemName });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<DrinkSet> Drinks { get; set; }
    public DbSet<FoodSet> Foods { get; set; }
}