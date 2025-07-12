using Microsoft.EntityFrameworkCore;

namespace RadzenThemeCustomizer.Api;

public class RadzenThemeCustomizerDbContext : DbContext
{
    public RadzenThemeCustomizerDbContext(DbContextOptions<RadzenThemeCustomizerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Theme>()
            .HasKey(t => t.Id);
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Themes)
            .WithOne()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }

    public DbSet<Theme> Themes { get; set; }
    public DbSet<User> Users { get; set; }
}
