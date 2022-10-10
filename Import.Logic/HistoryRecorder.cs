using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Записыватель историй о получении продуктов.
/// </summary>
public sealed class HistoryRecorder : IHistoryRecorder
{
    private readonly IRepository<History> _repository;
    private readonly ILogger<HistoryRecorder> _logger;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземляр типа <see cref="HistoryRecorder"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="repository" xml:lang = "ru">
    /// Репозиторий историй.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если один из параметров равен <see langword="null"/>.
    /// </exception>
    public HistoryRecorder(ILogger<HistoryRecorder> logger, IRepository<History> repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если история равна <see langword="null"/>.
    /// </exception>
    public async Task RecordHistoryAsync(
        IReadOnlyCollection<History> histories,
        CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(histories);

        token.ThrowIfCancellationRequested();

        var _mutex = new AsyncLock();
        using (await _mutex.LockAsync(token))
        {
            _logger.LogDebug("Start writing new history...");

            foreach (var history in histories)
            {
                await _repository.AddAsync(history, token);
            }

            _repository.Save();

            _logger.LogInformation(
                "New history: {History} be writed to database",
                string.Join('|', histories));
        }

    }
}
