using Import.Logic.Storage.Models;

using Microsoft.EntityFrameworkCore;

namespace Import.Logic.Storage;

/// <summary xml:lang = "ru">
/// Констекс базы данных сервиса "Импорта".
/// </summary>
public sealed class ImportContext : DbContext
{
    /// <summary xml:lang="ru">
    /// Создаёт новый экземпляр типа <see cref="ImportContext"/>.
    /// </summary>
    /// <param name="options" xml:lang="ru">
    /// Опции.
    /// </param>
    public ImportContext(DbContextOptions<ImportContext> options)
            : base(options)
    {
    }

    /// <summary xml:lang="ru">
    /// Истории.
    /// </summary>
    public DbSet<History> Histories { get; set; } = default!;

    /// <summary xml:lang="ru">
    /// Связи.
    /// </summary>
    public DbSet<Link> Links { get; set; } = default!;

    /// <summary xml:lang="ru">
    /// Провайдеры.
    /// </summary>
    public DbSet<Provider> Providers { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => new
            {
                e.ExternalId,
                e.ProviderId
            });

            entity.ToTable("histories");

            entity.Property(e => e.ExternalId)
                .HasColumnName("external_id");

            entity.Property(e => e.ProductMetadata)
                .HasMaxLength(80)
                .HasColumnName("product_metadata");

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
