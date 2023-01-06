using Microsoft.EntityFrameworkCore;

namespace pagination.Data;

public class DataDbContext : DbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DbSet<Seed> Seeds { get; set; }
    public DbSet<FirstExampleData> FirstExampleData { get; set; }
    public DbSet<SecondExampleData> SecondExampleData { get; set; }
    public DbSet<ThirdExampleData> ThirdExampleData { get; set; }
    public DbSet<ForthExampleData> ForthExampleData { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = "server=127.0.0.1;uid=root;pwd=root;database=pagination";
        optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection), options =>
        {
            options.EnableRetryOnFailure(10);
        })
            .EnableDetailedErrors(true);

        optionsBuilder.LogTo(Console.WriteLine, minimumLevel: Microsoft.Extensions.Logging.LogLevel.Warning);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seed>()
            .ToTable("__seeds")
            .HasKey(x => x.ExampleId);
        modelBuilder.Entity<FirstExampleData>()
            .ToTable("1_FirstExample");
        modelBuilder.Entity<SecondExampleData>()
            .ToTable("2_SecondExample");
        modelBuilder.Entity<ThirdExampleData>()
            .ToTable("3_ThirdExample");
        modelBuilder.Entity<ForthExampleData>()
            .ToTable("4_ForthExample");
    }
}