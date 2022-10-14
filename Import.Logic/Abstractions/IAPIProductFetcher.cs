using Import.Logic.Models;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает получателя продуктов по API.
/// </summary>
/// <typeparam name="TExternalProduct" xml:lang = "ru">
/// Маркерный тип внешнего продукта, который он обрабатывает.
/// </typeparam>
public interface IAPIProductFetcher<TExternalProduct>
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
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="request"/> имел некорректный формат.
    /// </exception>
    public IReadOnlyCollection<Product> FetchProducts(string request);
}
