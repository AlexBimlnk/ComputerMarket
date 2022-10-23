namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель продукта, используемая хранилищем.
/// </summary>
public sealed class Product
{
    /// <summary xml:lang = "ru">
    /// Индетификатор товара.
    /// </summary>
    public long ItemId { get; set; }

    /// <summary xml:lang = "ru">
    /// Цена установленная поставщиком. 
    /// </summary>
    public decimal ProviderCost { get; set; }

    /// <summary xml:lang = "ru">
    /// Коллиечество продукта.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary xml:lang = "ru">
    /// Индетификатор провайдера.
    /// </summary>
    public long ProviderId { get; set; }

    /// <summary xml:lang = "ru">
    /// Това.
    /// </summary>
    public Item Item { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Поставщик.
    /// </summary>
    public Provider Provider { get; set; } = null!;
}
