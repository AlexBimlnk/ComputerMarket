namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель описания товара, используемая хранилищнм.
/// </summary>
public sealed class ItemDescription
{
    /// <summary xml:lang = "ru">
    /// Индетификатор товара.
    /// </summary>
    public long ItemId { get; set; }

    /// <summary xml:lang = "ru">
    /// Индетификатор свойства.
    /// </summary>
    public long PropertyId { get; set; }

    /// <summary xml:lang = "ru">
    /// Значение свойства.
    /// </summary>
    public string? PropertyValue { get; set; }

    /// <summary xml:lang = "ru">
    /// Товар.
    /// </summary>
    public Item Item { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Свойство товара.
    /// </summary>
    public ItemProperty Property { get; set; } = null!;
}
