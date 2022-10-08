using Import.Logic.Storage.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Import.Logic.Storage.Repositories;

/// <summary xml:lang = "ru">
/// Контекст БД для репозиториев.
/// </summary>
public sealed class RepositoryContext : IRepositoryContext
{
    private readonly ImportContext _importContext;
    private readonly ILogger _logger;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="RepositoryContext"/>.
    /// </summary>
    /// <param name="importContext" xml:lang = "ru">
    /// Контекст БД импорта.
    /// </param>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    public RepositoryContext(ImportContext importContext, ILogger logger)
    {
        _importContext = importContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public DbSet<Link> Links => _importContext.Links;

    /// <inheritdoc/>
    public DbSet<History> Histories => _importContext.Histories;

    /// <inheritdoc/>
    public DbSet<Provider> Providers => _importContext.Providers;

    /// <inheritdoc/>
    public void SaveChanges()
    {
        _logger.LogDebug("Save changes");

        _importContext.SaveChanges();

        _logger.LogDebug("Changes sent to database");
    }
}
