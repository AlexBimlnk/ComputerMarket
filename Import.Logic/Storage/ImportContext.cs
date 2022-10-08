using Import.Logic.Storage.Models;

using Microsoft.EntityFrameworkCore;

namespace Import.Logic.Storage;

/// <summary xml:lang = "ru">
/// Констекс базы данных сервиса "Импорта".
/// </summary>
public sealed class ImportContext : DbContext
{
    public ImportContext(DbContextOptions<ImportContext> options)
            : base(options)
    {
    }

    public DbSet<History> Histories { get; set; } = default!;
    public DbSet<Link> Links { get; set; } = default!;
    public DbSet<Provider> Providers { get; set; } = default!;

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
            entity.HasKey(e => new
            {
                e.InternalId,
                e.ExternalId,
                e.ProviderId
            });

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
            entity.HasKey(e => e.Id);

            entity.ToTable("providers");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("provider_name");
        });
    }
}
