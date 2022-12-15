using Market.Logic.Storage.Models;

using Microsoft.EntityFrameworkCore;

using PropertyDataTypeId = Market.Logic.Models.PropertyDataType;

namespace Market.Logic.Storage;

/// <summary xml:lang = "ru">
/// Клнтекст базы данных сервиса "Маркета".
/// </summary>
public sealed class MarketContext : DbContext
{
    /// <summary  xml:lang = "ru">
    /// Создаёт экземпляр класса <see cref="MarketContext"/>.
    /// </summary>
    /// <param name="options"  xml:lang = "ru">Опции.</param>
    public MarketContext(DbContextOptions<MarketContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies();
        AppContext.
            SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        base.OnConfiguring(optionsBuilder);
    }

    /// <summary xml:lang = "ru">
    /// Товары.
    /// </summary>
    public DbSet<Item> Items { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Свойства товаров.
    /// </summary>
    public DbSet<ItemProperty> ItemProperties { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Тпиы пользователей.
    /// </summary>
    public DbSet<ItemType> ItemTypes { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Продукты.
    /// </summary>
    public DbSet<Product> Products { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Группы свойств.
    /// </summary>
    public DbSet<PropertyGroup> PropertyGroups { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Поставщики.
    /// </summary>
    public DbSet<Provider> Providers { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Пользователи.
    /// </summary>
    public DbSet<User> Users { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Типы пользователей.
    /// </summary>
    public DbSet<UserType> UserTypes { get; set; } = default!;

    /// <summary xml:lang="ru">
    /// Заказы.
    /// </summary>
    public DbSet<Order> Orders { get; set; } = default!;

    /// <summary xml:lang="ru">
    /// Продукты в корзинах пользователей.
    /// </summary>
    public DbSet<BasketItem> BasketItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("items");

            entity.HasKey(e => e.Id)
                .HasName("items_pkey");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");

            entity.Property(e => e.TypeId)
                .HasColumnName("type_id");

            entity.HasOne(d => d.Type)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("items_type_id_fkey");
        });

        modelBuilder.Entity<ItemDescription>(entity =>
        {
            entity.ToTable("item_description");

            entity.HasKey(e => new { e.ItemId, e.PropertyId })
                .HasName("item_description_pkey");

            entity.Property(e => e.ItemId).HasColumnName("item_id");

            entity.Property(e => e.PropertyId).HasColumnName("property_id");

            entity.Property(e => e.PropertyValue)
                .HasMaxLength(40)
                .HasColumnName("property_value")
                .HasDefaultValueSql("NULL::character varying");

            entity.HasOne(i => i.Item)
                .WithMany(d => d.Description)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("item_description_item_id_fkey");

            entity.HasOne(d => d.Property)
                .WithMany()
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("item_description_property_id_fkey");
        });

        modelBuilder.Entity<ItemProperty>(entity =>
        {
            entity.ToTable("item_properties");

            entity.HasKey(e => e.Id)
                .HasName("item_properties_pkey");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.GroupId).HasColumnName("group_id");

            entity.Property(e => e.IsFilterable).HasColumnName("is_filterable");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");

            entity.Property(e => e.PropertyDataTypeId)
                .HasColumnName("data_type_id");

            entity.HasOne(d => d.Group)
                .WithMany()
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("item_properties_group_id_fkey");
        });

        modelBuilder.Entity<PropertyDataType>(entity =>
        {
            entity.ToTable("property_data_type");

            entity.HasKey(e => e.PropertyDataTypeId)
                .HasName("property_data_type_pkey");

            entity.Property(e => e.PropertyDataTypeId).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");

            entity.HasData(Enum.GetValues(typeof(PropertyDataTypeId))
                .Cast<PropertyDataTypeId>().Select(e => new PropertyDataType()
                {
                    PropertyDataTypeId = e,
                    Name = e.ToString(),
                }));
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.ToTable("item_type");

            entity.HasKey(e => e.Id)
                .HasName("item_type_pkey");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");

            entity.HasMany(d => d.Properties)
                .WithMany(p => p.Types)
                .UsingEntity<Dictionary<string, object>>(
                    "ItemTypeProperty",
                    l => l.HasOne<ItemProperty>().WithMany().HasForeignKey("PropertyId").HasConstraintName("item_type_properties_property_id_fkey"),
                    r => r.HasOne<ItemType>().WithMany().HasForeignKey("TypeId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("item_type_properties_type_id_fkey"),
                    j =>
                    {
                        j.HasKey("TypeId", "PropertyId").HasName("item_type_properties_pkey");

                        j.ToTable("item_type_properties");

                        j.IndexerProperty<int>("TypeId").HasColumnName("type_id");

                        j.IndexerProperty<long>("PropertyId").HasColumnName("property_id");
                    });
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => new { e.ProviderId, e.ItemId })
                .HasName("product_pkey");

            entity.ToTable("products");

            entity.Property(e => e.ProviderId).HasColumnName("provider_id");

            entity.Property(e => e.ItemId).HasColumnName("item_id");

            entity.Property(e => e.ProviderCost)
                .HasPrecision(20, 2)
                .HasColumnName("provider_cost");

            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_item_id_fkey");

            entity.HasOne(d => d.Provider)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("product_provider_id_fkey");
        });

        modelBuilder.Entity<PropertyGroup>(entity =>
        {
            entity.ToTable("property_group");

            entity.HasKey(e => e.Id)
                .HasName("property_group_pkey");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.ToTable("providers");

            entity.HasKey(e => e.Id)
                .HasName("providers_pkey");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.BankAccount)
                .HasMaxLength(20)
                .HasColumnName("bank_account");

            entity.Property(e => e.Inn)
                .HasMaxLength(10)
                .HasColumnName("inn");

            entity.Property(e => e.Margin)
                .HasPrecision(5, 4)
                .HasColumnName("margin")
                .HasDefaultValueSql("1.0000");

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");

            entity.Property(e => e.IsAproved)
                .HasColumnName("is_approved")
                .HasDefaultValue(false);
        });

        modelBuilder.Entity<ProviderAgent>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ProviderId })
                .HasName("providers_agents_pkey");

            entity.ToTable("providers_agents");

            entity.HasIndex(e => e.UserId, "unique_user_check")
                .IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.ProviderId).HasColumnName("provider_id");

            entity.HasOne(d => d.Provider)
                .WithMany(p => p.ProvidersAgents)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("providers_agents_provider_id_fkey");

            entity.HasOne(d => d.User)
                .WithOne(p => p.ProvidersAgent)
                .HasForeignKey<ProviderAgent>(d => d.UserId)
                .HasConstraintName("providers_agents_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.Id)
                .HasName("users_pkey");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .HasColumnName("login");

            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");

            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");

            entity.Property(e => e.UserTypeId).HasColumnName("user_type_id");

            entity.HasOne(d => d.UserType)
                .WithMany()
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_user_type_id_fkey");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.ToTable("user_type");

            entity.HasKey(e => e.Id)
                .HasName("user_type_pkey");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(8)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(e => e.Id)
                .HasName("orders_pkey");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.Date)
                .HasColumnName("date");

            entity.Property(e => e.StateId)
                .HasColumnName("state_id");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("user_id_fkey");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("order_fill");

            entity.HasKey(e => new { e.OrderId, e.ItemId, e.ProviderId})
                .HasName("order_fill_pkey");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");

            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.Property(e => e.PaidPrice).HasColumnName("paid_price");

            entity.HasOne(d => d.Order)
                .WithMany(d => d.Items)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("order_fill_product_id_fkey");

            entity.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => new{ d.ItemId, d.ProviderId })
                .HasConstraintName("order_fill_item_id_fkey");
        });

        modelBuilder.Entity<BasketItem>(entity =>
        {
            entity.ToTable("basket_items");

            entity.HasKey(e => new { e.UserId, e.ItemId, e.ProviderId })
                .HasName("basket_items_pkey");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.ItemId).HasColumnName("item_id");

            entity.Property(e => e.ProviderId).HasColumnName("provider_id");

            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => new { d.ItemId, d.ProviderId })
                .HasConstraintName("basket_items_product_id_fkey");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("basket_items_user_id_fkey");
        });
    }
}