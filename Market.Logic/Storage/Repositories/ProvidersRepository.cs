using General.Storage;

using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Market.Logic.Storage.Repositories;

public sealed class ProvidersRepository : RepositoryHelper, IKeyableRepository<Provider, ID>
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<ProvidersRepository> _logger;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="ProvidersRepository"/>.
    /// </summary>
    /// <param name="context" xml:lang = "ru">
    /// Контекст репозиториев БД.
    /// </param>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из параметров равен <see langword="null"/>.
    /// </exception>
    public ProvidersRepository(
        IRepositoryContext context,
        ILogger<ProvidersRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
   

    /// <inheritdoc/>
    public async Task AddAsync(Provider entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _context.Providers.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(Provider entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Providers.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Delete(Provider entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Providers.Remove(ConvertToStorageModel(entity));
    }

    /// <inheritdoc/>
    public IEnumerable<Provider> GetEntities() =>
        _context.Providers
        .AsEnumerable()
        .Select(x => ConvertFromStorageModel(x))
        .Where(x => x != null)!;

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    /// <inheritdoc/>
    public Provider? GetByKey(ID key) =>
        _context.Providers
            .Where(x => x.Id == key.Value)
            .Select(x => ConvertFromStorageModel(x))
            .SingleOrDefault();
}
