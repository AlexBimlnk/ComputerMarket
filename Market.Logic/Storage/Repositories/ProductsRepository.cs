using General.Storage;

using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace Market.Logic.Storage.Repositories;

public sealed class ProductsRepository : RepositoryHelper, IKeyableRepository<Product, (long, long)>
{
    private readonly ILogger<ProductsRepository> _logger;
    private readonly IRepositoryContext _context;

    public ProductsRepository(IRepositoryContext context, ILogger<ProductsRepository> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    

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
