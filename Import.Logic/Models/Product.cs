using Import.Logic.Abstractions;

namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Продукт.
/// </summary>
public sealed class Product: IMappableEntity<InternalID, ExternalID>
{
    private InternalID? _internalID;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="Product"/>
    /// </summary>
    /// <param name="externalID" xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </param>
    /// <param name="price" xml:lang = "ru">
    /// Цена.
    /// </param>
    /// <param name="quantity" xml:lang = "ru">
    /// Количество.
    /// </param>
    /// <param name="internalID" xml:lang = "ru">
    /// Внутренний идентификатор продукта.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Когда <paramref name="quantity"/> меньше нуля.
    /// </exception>
    public Product(
        ExternalID externalID,
        Price price,
        int quantity)
    {
        ExternalID = externalID;
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
    public InternalID InternalID =>
        _internalID ?? throw new InvalidOperationException("Product isn't mapped.");

    /// <summary xml:lang = "ru">
    /// Цена.
    /// </summary>
    public Price Price { get; }

    /// <summary xml:lang = "ru">
    /// Количество.
    /// </summary>
    public int Quantity { get; }

    ///<inheritdoc/>
    public bool IsMapped { get; private set; }


    /// <summary xml:lang = "ru">
    /// Устанавливает связь с внутренним продуктом.
    /// </summary>
    /// <param name="internalID" xml:lang = "ru">
    /// Идентификатор внутреннего продукта.
    /// </param>
    public void MapTo(InternalID internalID)
    {
        if (IsMapped)
            throw new InvalidOperationException("Product already is mapped.");

        _internalID = internalID;
        IsMapped = true;
    }
}