namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Представляет цену продукта.
/// </summary>
public struct Price
{
    /// <summary xml:lang = "ru">
    /// Не поддерживаемый конструктор цены.
    /// </summary>
    /// <exception cref="NotSupportedException"/>
    public Price()
    {
        throw new NotSupportedException();
    }

    /// <summary xml:lang = "ru">
    /// Создает новый экемпляр типа <see cref="Price"/>.
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
    public decimal Value { get; }
}
