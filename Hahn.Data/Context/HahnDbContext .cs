using Hahn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hahn.Data.Context;

public class HahnDbContext : DbContext
{
    public HahnDbContext(DbContextOptions<HahnDbContext> options)
        : base(options) { }

    public DbSet<FoodRecipe> FoodRecipes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}