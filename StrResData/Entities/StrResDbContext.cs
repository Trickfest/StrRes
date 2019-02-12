using Microsoft.EntityFrameworkCore;

namespace StrResData.Entities
{
    public class StrResDbContext : DbContext
    {
        public StrResDbContext(DbContextOptions<StrResDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Resource>()
                .HasKey(r => new
                {
                    r.TenantId,
                    r.Key
                });

            builder.Entity<Resource>()
                .HasOne(r => r.Tenant)
                .WithMany(t => t.Resources)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Tenant>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }

        public DbSet<StrResData.Entities.Tenant> Tenant { get; set; }
        public DbSet<StrResData.Entities.Resource> Resource { get; set; }
        public DbSet<StrResData.Entities.Admin> Admin { get; set; }
    }
}