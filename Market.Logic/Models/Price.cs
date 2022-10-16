namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет цену продукта.
/// </summary>
public struct Price
{
    /// <summary xml:lang = "ru">
    /// Создает новый экемпляр типа <see cref="ProductPrice"/>.
    /// </summary>
    /// <param name="price" xml:lang = "ru">
    /// Цена продукта.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Если <paramref name="price"/> меньше или равен нулю.
    /// </exception>
    public Price(decimal price)
    {
        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price));

        Value = price;
    }

    /// <summary xml:lang = "ru">
    /// Значение.
    /// </summary>
    public decimal Value { get; private set; }
}