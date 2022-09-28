using Import.Logic.Storage.Models;

using Microsoft.EntityFrameworkCore;

namespace Import.Logic.Storage;
public class ImportContext : DbContext
{
    public ImportContext(DbContextOptions<ImportContext> options)
            : base(options)
    {
    }

    public DbSet<History> ExternalProducts { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<History>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("histories");

            entity.Property(e => e.ExternalId)
                .HasColumnName("external_id");

            entity.Property(e => e.ProductDescription)
                .HasMaxLength(50)
                .HasColumnName("product_description");

            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnName("product_name");

            entity.Property(e => e.ProviderId)
                .HasColumnName("provider_id");
        });

        modelBuilder.Entity<Link>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("links");

            entity.Property(e => e.ExternalId)
                .HasColumnName("external_id");

            entity.Property(e => e.InternalId)
                .HasColumnName("internal_id");

            entity.Property(e => e.ProviderId)
                .HasColumnName("provider_id");

            entity.HasOne(d => d.Provider)
                .WithMany()
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("links_provider_id_fkey");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.ToTable("providers");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("provider_name");
        });
    }
}
