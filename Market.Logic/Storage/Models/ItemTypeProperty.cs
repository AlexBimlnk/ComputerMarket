namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель свйоств относящихся к типу товара, используемая хранилищем.
/// </summary>
public sealed class ItemTypeProperty
{
    /// <summary xml:lang = "ru">
    /// Индетификатор типа товаров.
    /// </summary>
    public int TypeId { get; set; }

    /// <summary xml:lang = "ru">
    /// Индетификатор свойтва товара.
    /// </summary>
    public long PropertyId { get; set; }

    /// <summary xml:lang = "ru">
    /// Свойство товара.
    /// </summary>
    public ItemProperty Property { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Тип товара.
    /// </summary>
    public ItemType Type { get; set; } = null!;
}
