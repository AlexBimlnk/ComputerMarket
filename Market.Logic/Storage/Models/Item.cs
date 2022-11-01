namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель товара, используемая хранилищем.
/// </summary>
public class Item
{
    /// <summary xml:lang = "ru">
    /// Индетификатор товара.
    /// </summary>
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название товара.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Индетификатор типа товара.
    /// </summary>
    public int TypeId { get; set; }

    /// <summary xml:lang = "ru">
    /// Тип товара.
    /// </summary>
    public virtual ItemType Type { get; set; } = null!;

    /// <summary xml:lang = "ru">
    ///  Продукты относящиеся к данному товару.
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

    /// <summary xml:lang = "ru">
    ///  Свойства данного товара.
    /// </summary>
    public virtual ICollection<ItemDescription> Description { get; set; } = new HashSet<ItemDescription>();
}
