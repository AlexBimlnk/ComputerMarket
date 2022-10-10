using Import.Logic.Models;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает записывателя историй о получении продуктов.
/// </summary>
public interface IHistoryRecorder
{
    /// <summary xml:lang = "ru">
    /// Записывает историю получения продуктов в БД.
    /// </summary>
    /// <param name="histories" xml:lang = "ru">
    /// История.
    /// </param>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    /// <exception cref="OperationCanceledException" xml:lang = "ru">
    /// Когда операция была отменена.
    /// </exception>
    public Task RecordHistoryAsync(IReadOnlyCollection<History> histories, CancellationToken token = default);
}
