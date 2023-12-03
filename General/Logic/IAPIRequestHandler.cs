namespace General.Logic;

/// <summary xml:lang = "ru">
/// Описывает обработчика запросов пришедших по API.
/// </summary>
/// <typeparam name="TMarker" xml:lang = "ru">
/// Маркерный тип обработчика запросов.
/// </typeparam>
public interface IAPIRequestHandler<TMarker>
{
    /// <summary xml:lang = "ru">
    /// Обрабатывает запрос.
    /// </summary>
    /// <param name="request" xml:lang = "ru">
    /// Входящий запрос.
    /// </param>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены операции.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="request"/> имел некорректный формат.
    /// </exception>
    /// <exception cref="OperationCanceledException" xml:lang = "ru">
    /// Когда операция была отменена.
    /// </exception>
    public Task HandleAsync(string request, CancellationToken token = default);
}

