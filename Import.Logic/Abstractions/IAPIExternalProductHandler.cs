namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает обработчика внешних продуктов, пришедших по API.
/// </summary>
/// <typeparam name="TExternalProduct" xml:lang = "ru">
/// Маркерный тип внешних продуктов.
/// </typeparam>
public interface IAPIExternalProductHandler<TExternalProduct>
{
    /// <summary xml:lang = "ru">
    /// Обрабатывает запрос с пришедшими извне продуктами.
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
