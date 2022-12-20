using Market.Logic.Storage.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Market.Logic.Storage.Repositories;
public sealed class RepositoryContext : IRepositoryContext
{
    private readonly MarketContext _marketContext;
    private readonly ILogger<RepositoryContext> _logger;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="RepositoryContext"/>.
    /// </summary>
    /// <param name="marketContext" xml:lang = "ru">
    /// Контекст БД маркета.
    /// </param>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    public RepositoryContext(
        MarketContext marketContext,
        ILogger<RepositoryContext> logger)
    {
        _marketContext = marketContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public DbSet<Item> Items => _marketContext.Items;

    /// <inheritdoc/>
    public DbSet<ItemProperty> ItemProperties => _marketContext.ItemProperties;

    /// <inheritdoc/>
    public DbSet<ItemType> ItemTypes => _marketContext.ItemTypes;

    /// <inheritdoc/>
    public DbSet<Product> Products => _marketContext.Products;

    /// <inheritdoc/>
    public DbSet<PropertyGroup> PropertyGroups => _marketContext.PropertyGroups;

    /// <inheritdoc/>
    public DbSet<Provider> Providers => _marketContext.Providers;

    /// <inheritdoc/>
    public DbSet<User> Users => _marketContext.Users;

    /// <inheritdoc/>
    public DbSet<UserType> UserTypes => _marketContext.UserTypes;

    /// <inheritdoc/>
    public DbSet<Order> Orders => _marketContext.Orders;

    /// <inheritdoc/>
    public DbSet<BasketItem> BasketItems => _marketContext.BasketItems;

    /// <inheritdoc/>
    public void SaveChanges()
    {
        _logger.LogDebug("Save changes");

        _marketContext.SaveChanges();

        _logger.LogDebug("Changes sent to database");
    }
}
