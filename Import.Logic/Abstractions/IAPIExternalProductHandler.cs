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
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public Task HandleAsync(string request);
}
