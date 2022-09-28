using Import.Logic.Models;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает получателя продуктов.
/// </summary>
/// <typeparam name="TExternalProductModel" xml:lang = "ru">
/// Тип модели продукта поставщика.
/// </typeparam>
public interface IProductFetcher
{
    /// <summary xml:lang = "ru">
    /// Получает список продуктов поставщика.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Коллекцию типа <see cref="IReadOnlyCollection{T}"/>.
    /// </returns>
    public IReadOnlyCollection<Product> FetchProducts();
}
