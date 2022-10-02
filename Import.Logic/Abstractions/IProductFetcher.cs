using Import.Logic.Models;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает получателя продуктов по API.
/// </summary>
public interface IAPIProductFetcher
{
    /// <summary xml:lang = "ru">
    /// Получает список продуктов поставщика.
    /// </summary>
    /// <param name="request" xml:lang = "ru">
    /// Запрос.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Коллекцию внутренних продуктов типа <see cref="Product"/>.
    /// </returns>
    public IReadOnlyCollection<Product> FetchProducts(string request);
}
