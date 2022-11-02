namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
///  Тип товара.
/// </summary>
public sealed record class ItemType
{
    private readonly int? _id;

    /// <summary>
    /// Создаёт экземпляр типа <see cref="ItemType"/>.
    /// </summary>
    /// <param name="name" xml:lang = "ru">Название типа.</param>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="name"/> - пустое или <see langword="null"/> или содержит только пробелы.
    /// </exception>
    public ItemType(string name, int? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name of {nameof(name)} can't be empty or null or contains only white spaces");
        Name = name;
        _id = id;
    }

    /// <summary xml:lang = "ru">
    /// Название типа.
    /// </summary>
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// Индетификатор типа продукта.
    /// </summary>
    public int Id => _id ?? throw new InvalidOperationException($"Key for {nameof(ItemType)} is not defined");
}
