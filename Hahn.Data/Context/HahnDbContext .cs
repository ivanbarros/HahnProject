using Hahn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hahn.Data.Context;

public class HahnDbContext : DbContext
{
    public HahnDbContext(DbContextOptions<HahnDbContext> options)
        : base(options) { }

    public DbSet<FoodRecipies> FoodRecipies { get; set; }
    


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodRecipies>()
            .ToTable("FoodRecipies");
        
        modelBuilder.Entity<Events>()
            .ToTable("Events");
    }
}