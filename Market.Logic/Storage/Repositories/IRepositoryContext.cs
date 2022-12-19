using Market.Logic.Storage.Models;

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
    public DbSet<Item> Items { get; }

    /// <summary xml:lang = "ru">
    /// Свойства товаров.
    /// </summary>
    public DbSet<ItemProperty> ItemProperties { get; }

    /// <summary xml:lang = "ru">
    /// Тпиы пользователей.
    /// </summary>
    public DbSet<ItemType> ItemTypes { get; }

    /// <summary xml:lang = "ru">
    /// Продукты.
    /// </summary>
    public DbSet<Product> Products { get; }

    /// <summary xml:lang = "ru">
    /// Группы свойств.
    /// </summary>
    public DbSet<PropertyGroup> PropertyGroups { get; }

    /// <summary xml:lang = "ru">
    /// Поставщики.
    /// </summary>
    public DbSet<Provider> Providers { get; }

    /// <summary xml:lang = "ru">
    /// Пользователи.
    /// </summary>
    public DbSet<User> Users { get; }

    /// <summary xml:lang = "ru">
    /// Типы пользователей.
    /// </summary>
    public DbSet<UserType> UserTypes { get; }

    /// <summary xml:lang="ru">
    /// Заказы.
    /// </summary>
    public DbSet<Order> Orders { get; }

    /// <summary xml:lang="ru">
    /// Продукты в корзинах пользователей.
    /// </summary>
    public DbSet<BasketItem> BasketItems { get; }

    /// <summary>
    /// Подтверждения заказов от провайдеров
    /// </summary>
    public DbSet<ProviderAprove> ProviderAproves { get; }

    /// <summary xml:lang = "ru">
    /// Сохраняет текущие изменения.
    /// </summary>
    public void SaveChanges();
}
