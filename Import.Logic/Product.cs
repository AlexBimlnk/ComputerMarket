namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Продукт.
/// </summary>
public sealed class Product
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="Product"/>
    /// </summary>
    /// <param name="externalID" xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </param>
    /// <param name="internalID" xml:lang = "ru">
    /// Внутренний идентификатор продукта.
    /// </param>
    /// <param name="price" xml:lang = "ru">
    /// Цена.
    /// </param>
    /// <param name="quantity" xml:lang = "ru">
    /// Количество.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Когда <paramref name="quantity"/> меньше нуля.
    /// </exception>
    public Product(
        ExternalID externalID, 
        InternalID? internalID, 
        Price price, 
        int quantity)
    {
        ExternalID = externalID;
        InternalID = internalID;
        Price = price;

        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Quantity = quantity;
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </summary>
    public ExternalID ExternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор продукта.
    /// </summary>
    public InternalID? InternalID { get; }

    /// <summary xml:lang = "ru">
    /// Цена.
    /// </summary>
    public Price Price { get; }

    /// <summary xml:lang = "ru">
    /// Количество.
    /// </summary>
    public int Quantity { get; }
}