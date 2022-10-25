

using Microsoft.EntityFrameworkCore;

namespace Market.Logic.Storage.Repositories;

/// <summary xml:lang = "ru">
/// Описывет контекст БД для репозиториев.
/// </summary>
public interface IRepositoryContext
{
    /// <summary xml:lang = "ru">
    /// Товары.
    /// </summary>
    public DbSet<Item> Items { get; set; }

    /// <summary xml:lang = "ru">
    /// Описания товаров.
    /// </summary>
    public DbSet<ItemDescription> ItemDescriptions { get; set; }

    /// <summary xml:lang = "ru">
    /// Свойства товаров.
    /// </summary>
    public DbSet<ItemProperty> ItemProperties { get; set; }

    /// <summary xml:lang = "ru">
    /// Тпиы пользователей.
    /// </summary>
    public DbSet<ItemType> ItemTypes { get; set; }

    /// <summary xml:lang = "ru">
    /// Продукты.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary xml:lang = "ru">
    /// Группы свойств.
    /// </summary>
    public DbSet<PropertyGroup> PropertyGroups { get; set; }

    /// <summary xml:lang = "ru">
    /// Поставщики.
    /// </summary>
    public DbSet<Provider> Providers { get; set; }

    /// <summary xml:lang = "ru">
    /// Пользователи.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary xml:lang = "ru">
    /// Типы пользователей.
    /// </summary>
    public DbSet<UserType> UserTypes { get; set; }

    /// <summary xml:lang = "ru">
    /// Представители поставщиков.
    /// </summary>
    public DbSet<ProviderAgent> ProviderAgents { get; set; }


    /// <summary xml:lang = "ru">
    /// Сохраняет текущие изменения.
    /// </summary>
    public void SaveChanges();
}
