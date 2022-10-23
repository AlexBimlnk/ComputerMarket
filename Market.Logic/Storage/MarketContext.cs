﻿using Market.Logic.Storage.Models;
using Microsoft.EntityFrameworkCore;

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

    /// <summary xml:lang = "ru">
    /// Товары.
    /// </summary>
    public DbSet<Item> Items { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Описания товаров.
    /// </summary>
    public DbSet<ItemDescription> ItemDescriptions { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Свойства товаров.
    /// </summary>
    public DbSet<ItemProperty> ItemProperties { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Тпиы пользователей.
    /// </summary>
    public DbSet<ItemType> ItemTypes { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Свойства относящиеся к типу товаров.
    /// </summary>
    public DbSet<ItemTypeProperty> ItemTypeProperties { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Продукты.
    /// </summary>
    public DbSet<Product> Products { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// 
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

    /// <summary xml:lang = "ru">
    /// 
    /// </summary>
    public DbSet<ProviderAgent> ProviderAgents { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("items");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("items_type_id_fkey");
            });

            modelBuilder.Entity<ItemDescription>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("item_description");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.PropertyId).HasColumnName("property_id");

                entity.Property(e => e.PropertyValue)
                    .HasMaxLength(40)
                    .HasColumnName("property_value")
                    .HasDefaultValueSql("NULL::character varying");

                entity.HasOne(d => d.Item)
                    .WithMany()
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("item_description_item_id_fkey");

                entity.HasOne(d => d.Property)
                    .WithMany()
                    .HasForeignKey(d => d.PropertyId)
                    .HasConstraintName("item_description_property_id_fkey");
            });

            modelBuilder.Entity<ItemProperty>(entity =>
            {
                entity.ToTable("item_properties");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.IsFilterable).HasColumnName("is_filterable");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");

                entity.Property(e => e.PropertyDataType)
                    .HasMaxLength(10)
                    .HasColumnName("property_data_type")
                    .HasDefaultValueSql("'varchar'::character varying");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ItemProperties)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("item_properties_group_id_fkey");
            });

            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.ToTable("item_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<ItemTypeProperty>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("item_type_properties");

                entity.Property(e => e.PropertyId).HasColumnName("property_id");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Property)
                    .WithMany()
                    .HasForeignKey(d => d.PropertyId)
                    .HasConstraintName("item_type_properties_property_id_fkey");

                entity.HasOne(d => d.Type)
                    .WithMany()
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("item_type_properties_type_id_fkey");
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
                    .HasConstraintName("product_provider_id_fkey");
            });

            modelBuilder.Entity<PropertyGroup>(entity =>
            {
                entity.ToTable("property_group");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("providers");

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
            });

            modelBuilder.Entity<ProviderAgent>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ProviderId })
                    .HasName("providers_agents_pkey");

                entity.ToTable("providers_agents");

                entity.HasIndex(e => e.UserId, "user_unique")
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

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Login)
                    .HasMaxLength(20)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .HasColumnName("password");

                entity.Property(e => e.UserTypeId).HasColumnName("user_type_id");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("users_user_type_id_fkey");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("user_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(8)
                    .HasColumnName("name");
            });
        }
    }
}