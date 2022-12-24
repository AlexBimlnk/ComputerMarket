namespace Market.Logic.Models;

/// <summary>
/// Обновление по продукту.
/// </summary>
public sealed class UpdateByProduct
{
    /// <summary>
    /// Создает объект типа <see cref="UpdateByProduct"/>.
    /// </summary>
    /// <param name="externalID">
    /// Внешний идентификатор продукта.
    /// </param>
    /// <param name="internalID">
    /// Внутренний идентификатор продукта.
    /// </param>
    /// <param name="providerID">
    /// Идентификатор поставщика.
    /// </param>
    /// <param name="price">
    /// Цена продукта.
    /// </param>
    /// <param name="quantity">
    /// Количество продукта.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="quantity"/> оказался меньше нуля.
    /// </exception>
    public UpdateByProduct(
        ID externalID,
        ID internalID,
        ID providerID,
        Price price,
        int quantity)
    {
        ExternalID = externalID;
        InternalID = internalID;
        ProviderID = providerID;
        Price = price;

        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity can't be less then 0");

        Quantity = quantity;
    }

    /// <summary xml:lang = "ru">
    /// Внешний индетификатор продукта.
    /// </summary>
    public ID ExternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внутрений индетификатор продукта.
    /// </summary>
    public ID InternalID { get; }

    /// <summary xml:lang = "ru">
    /// Название поставщика.
    /// </summary>
    public ID ProviderID { get; }

    /// <summary xml:lang = "ru">
    /// Цена продукта.
    /// </summary>
    public Price Price { get; }

    /// <summary xml:lang = "ru">
    /// Количество продукта.
    /// </summary>
    public int Quantity { get; }
}
