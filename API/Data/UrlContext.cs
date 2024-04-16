using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UrlContext : IdentityDbContext<User, Role, int>
{
    public UrlContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberofCharsInShortLink);
            builder.HasIndex(s => s.Code).IsUnique();
        }); 
        
        builder.Entity<Role>()
            .HasData(
                new Role {Id = 1, Name = "Member", NormalizedName = "MEMBER"},
                new Role {Id = 2, Name = "Admin", NormalizedName = "ADMIN"}
            );
    }
}