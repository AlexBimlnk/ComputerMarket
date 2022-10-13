using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Записыватель историй о получении продуктов.
/// </summary>
public sealed class HistoryRecorder : IHistoryRecorder
{
    private readonly ILogger<HistoryRecorder> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземляр типа <see cref="HistoryRecorder"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="scopeFactory" xml:lang = "ru">
    /// Фабрика сервисов с областью применения.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если один из параметров равен <see langword="null"/>.
    /// </exception>
    public HistoryRecorder(
        ILogger<HistoryRecorder> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    /// <inheritdoc/>
    public async Task RecordHistoryAsync(
        Product product,
        CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(product);

        token.ThrowIfCancellationRequested();

        var _mutex = new AsyncLock();
        using (await _mutex.LockAsync(token).ConfigureAwait(false))
        {
            _logger.LogDebug("Start writing new history...");

            using var scope = _scopeFactory.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IRepository<History>>();

            var history = new History(product.ExternalID, product.Metadata);

            await repository.AddAsync(history, token)
                    .ConfigureAwait(false);

            repository.Save();

            _logger.LogDebug(
                "New history: {History} be writed to database",
                history);
        }

    }
}
