using Microsoft.EntityFrameworkCore;

namespace pagination.Data;

public class DataDbContext : DbContext
{
    public DbSet<Seed>? Seeds;

    public DbSet<FirstExampleData>? FirstExampleData { get; set; }
    public DbSet<SecondExampleData>? SecondExampleData { get; set; }
    public DbSet<ThirdExampleData>? ThirdExampleData { get; set; }
    public DbSet<ForthExampleData>? ForthExampleData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = "server=127.0.0.1;uid=root;pwd=root;database=pagination";
        optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seed>()
            .HasNoKey()
            .ToTable("__seeds");
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