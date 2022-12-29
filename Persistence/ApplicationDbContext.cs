using Domain.Common;
using Domain.Data.Entities;
using Domain.Data.Queries;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDbContext : IdentityDbContext<ApiUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<FileUpload> FileUploads { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<DepthQuery> DepthQuery { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApiUser>().Property(u => u.ChangePassword).HasDefaultValue(true);

        builder.Entity<Page>().HasMany(p => p.Children).WithOne(p => p.Parent).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Page>().HasMany(p => p.Files).WithOne(f => f.Page).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Post>().HasMany(p => p.Files).WithOne(f => f.Post).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Post>().HasOne(p => p.Page).WithMany(p => p.Posts).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Page>().HasData(new Page
        {
            Id = 1, Title = "Index", Description = "Index description",
            Content = "Index content, only shown on the page itself",
            Visible = true, CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        });

        builder.Entity<DepthQuery>().HasNoKey().ToView(null);

        base.OnModelCreating(builder);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added) ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
        }
    }
}