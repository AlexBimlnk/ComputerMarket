using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Market.Logic.Storage.Repositories;

public sealed class ItemsRepository : RepositoryHelper, IItemsRepository
{
    private readonly ILogger<ItemsRepository> _logger;
    private readonly IRepositoryContext _context;

    public ItemsRepository(IRepositoryContext context, ILogger<ItemsRepository> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    

    /// <inheritdoc/>
    public async Task AddAsync(Item entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _context.Items.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(Item entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Items.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Delete(Item entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Items.Remove(ConvertToStorageModel(entity));
    }

    /// <inheritdoc/>
    public IEnumerable<Item> GetEntities() =>
        _context.Items
        .AsEnumerable()
        .Select(x => ConvertFromStorageModel(x));

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    /// <inheritdoc/>
    public Item? GetByKey(ID key) =>
        _context.Items
            .Where(x => x.Id == key.Value)
            .Select(x => ConvertFromStorageModel(x))
            .SingleOrDefault();

    /// <inheritdoc/>
    public void AddOrUpdate(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _context.Items.Update(ConvertToStorageModel(item));
    }
}
