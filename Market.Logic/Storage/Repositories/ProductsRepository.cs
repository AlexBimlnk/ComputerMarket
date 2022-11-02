using General.Storage;

using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using TProduct = Market.Logic.Storage.Models.Product;


namespace Market.Logic.Storage.Repositories;

public sealed class ProductsRepository : IKeyableRepository<Product, (long, long)>
{
    private readonly ILogger<ProductsRepository> _logger;
    private readonly IRepositoryContext _context;

    public ProductsRepository(IRepositoryContext context, ILogger<ProductsRepository> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private static TProduct ConvertToStorageModel(Product product) => new()
    {
        ProviderCost = product.ProviderCost,
        Quantity = product.Quantity,
        ItemId = product.Item.Key.Value,
        ProviderId = product.Provider.Key.Value,
    };

    private static Product ConvertFromStorage(TProduct product)
      => new Product(
          new Item(
              new InternalID(product.ItemId),
              new ItemType(product.Item.Type.Name),
              product.Item.Name,
              product.Item.Description
                .Select(x => new ItemProperty(
                    x.Property.Name,
                    x.PropertyValue ?? string.Empty))
                .ToArray()),
          new Provider(
              new InternalID(product.ProviderId),
              product.Provider.Name,
              new Margin(product.Provider.Margin),
              new PaymentTransactionsInformation(
                  product.Provider.Inn,
                  product.Provider.BankAccount)),
          new Price(product.ProviderCost),
          product.Quantity);

    /// <inheritdoc/>
    public async Task AddAsync(Product entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _context.Products.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(Product entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Products.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Delete(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Products.Remove(ConvertToStorageModel(entity));
    }

    /// <inheritdoc/>
    public IEnumerable<Product> GetEntities() =>
        _context.Products
        .AsEnumerable()
        .Select(x => ConvertFromStorage(x));

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    /// <inheritdoc/>
    public Product? GetByKey((long, long) key) =>
        _context.Products
            .Where(x => x.ItemId == key.Item1 && x.ProviderId == key.Item2)
            .Select(x => ConvertFromStorage(x))
            .SingleOrDefault();
}
