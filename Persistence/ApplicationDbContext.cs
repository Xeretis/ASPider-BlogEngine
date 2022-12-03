using Domain.Common;
using Domain.Data.Entities;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApiUser>().Property(u => u.ChangePassword).HasDefaultValue(true);
        //builder.Entity<Page>().Navigation(p => p.Files).AutoInclude();
        //builder.Entity<Page>().Navigation(p => p.Posts).AutoInclude();

        builder.Entity<Page>().HasData(new Page
        {
            Id = 1, Title = "Index", Description = "Index description",
            Content = "Index content, only shown on the page itself",
            Visible = true, CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        });

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