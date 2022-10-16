using General.Storage;

using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Сервис инициализации кэша связей.
/// </summary>
public sealed class CacheInizializerService : IHostedService
{
    private readonly ILogger<CacheInizializerService> _logger;
    private readonly ICache<Link> _cache;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземляр типа <see cref="CacheInizializerService"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="cache" xml:lang = "ru">
    /// Кэш.
    /// </param>
    /// <param name="scopeFactory" xml:lang = "ru">
    /// Фабрика сервисов с областью применения.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public CacheInizializerService(
        ILogger<CacheInizializerService> logger,
        ICache<Link> cache,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogDebug("Starting inizialize cache...");

        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Link>>();

        _cache.AddRange(repository.GetEntities());

        _logger.LogInformation("The cache has been initialized.");

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }
}
