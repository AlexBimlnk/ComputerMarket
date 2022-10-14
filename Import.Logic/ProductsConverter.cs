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

        var metadata = 
            $"{source.Name}" +
            $"{(source.Description is null ? string.Empty : string.Join("|", source.Description))}";

        return new Product(
            new ExternalID(source.Id, Provider.Ivanov),
            new Price(source.Price),
            source.Quantity,
            metadata);
    }

    /// <inheritdoc/>
    public Product Convert(HornsAndHoovesProduct source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var metadata = $"{source.Name}|{source.Description}|{source.Type}";

        return new Product(
            new ExternalID(source.Id, Provider.HornsAndHooves),
            new Price(source.Price),
            source.Quantity,
            metadata);
    }
}
