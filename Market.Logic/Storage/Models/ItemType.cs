namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель типа товара, используемая хранилищем.
/// </summary>
public sealed class ItemType
{
    /// <summary xml:lang = "ru">
    /// Индетификатор типа товара.
    /// </summary>
    public int Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название типа товара.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Товары относящиеся к данному типу.
    /// </summary>
    public ICollection<Item> Items { get; set; } = new HashSet<Item>();
}
