namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет описание товара.
/// </summary>
public sealed class Item : IEquatable<Item>
{
    private readonly long? _id;

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="Item"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">Тип товара.</param>
    /// <param name="name" xml:lang = "ru">Название товара.</param>
    /// <param name="properties" xml:lang = "ru"></param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="properties"/> или <paramref name="type"/> - <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    ///  Если <paramref name="name"/> - состоит из пробелов, явялется пустой строкой или <see langword="null"/>.
    /// </exception>
    public Item(ItemType type, string name, IReadOnlyCollection<ItemProperty> properties, long? id = null)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name of {nameof(Item)} can't be null or white spaces or empty.");

        Name = name;
        Properties = properties ?? throw new ArgumentNullException(nameof(type));
        _id = id;
    }

    /// <summary xml:lang = "ru">
    /// Тип товара.
    /// </summary>
    public ItemType Type { get; }

    /// <summary xml:lang = "ru">
    /// Название товара.
    /// </summary>
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// Характеристики товара.
    /// </summary>
    public IReadOnlyCollection<ItemProperty> Properties { get; }

    /// <summary xml:lang = "ru">
    /// Индетификатор товара.
    /// </summary>
    public long Id => _id ?? throw new InvalidOperationException($"Key for {nameof(Item)} is not defined");

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Type, Name, Properties);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Item item && Equals(item);

    /// <inheritdoc/>
    public bool Equals(Item? other) =>
        Name.Equals(other?.Name) &&
        Type.Equals(other?.Type) &&
        Properties.Equals(other?.Properties);
}
