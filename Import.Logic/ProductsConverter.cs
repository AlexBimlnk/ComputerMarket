using Import.Logic.Abstractions;
using Import.Logic.Models;
using Import.Logic.Transport.Models;

namespace Import.Logic;

/// <summary xml:lang="ru">
/// Конвертер продуктов.
/// </summary>
public sealed class ProductsConverter : 
    IConverter<ExternalProduct, Product>,
    IConverter<HornsAndHoovesProduct, Product>
{
    /// <inheritdoc/>
    public Product Convert(ExternalProduct source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new Product(
            new ExternalID(source.Id, Provider.Ivanov),
            new Price(source.Price),
            source.Quantity);
    }

    /// <inheritdoc/>
    public Product Convert(HornsAndHoovesProduct source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new Product(
            new ExternalID(source.Id, Provider.HornsAndHooves),
            new Price(source.Price),
            source.Quantity);
    }
}
