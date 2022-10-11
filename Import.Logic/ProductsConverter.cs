using Import.Logic.Abstractions;
using Import.Logic.Models;
using Import.Logic.Transport.Models;

namespace Import.Logic;

/// <summary xml:lang="ru">
/// Конвертер продуктов.
/// </summary>
public sealed class ProductsConverter : IConverter<ExternalProduct, Product>
{
    /// <summary xml:lang="ru">
    /// Конвертирует внешние продукта типа <see cref="ExternalProduct"/> во внутренние.
    /// </summary>
    /// <param name="source" xml:lang="ru">
    /// Продукт типа <see cref="ExternalProduct"/>, который нужно сконвертировать.
    /// </param>
    /// <returns xml:lang="ru">
    /// Сконвертированный продукт типа <see cref="Product"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang="ru">
    /// Когда <paramref name="source"/> был <see langword="null"/>.
    /// </exception>
    public Product Convert(ExternalProduct source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new Product(
            new ExternalID(source.Id, Provider.Ivanov),
            new Price(source.Price),
            source.Quantity);
    }
}
