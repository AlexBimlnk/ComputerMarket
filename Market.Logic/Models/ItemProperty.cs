namespace Market.Logic.Models;

/// <summary>
/// Свойтсво товара.
/// </summary>
public sealed class ItemProperty
{
    /// <summary>
    /// Название свойства.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Значения свойства.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создает экземпляр типа <see cref="ItemProperty"/>.
    /// </summary>
    /// <param name="name">Название свойства.</param>
    /// <param name="value">Значение свойства.</param>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="name"/> или <paramref name="value"/> - пустые, состоят из пробелов или <see langword="null"/>.
    /// </exception>
    public ItemProperty(string name, string value)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name of {nameof(ItemProperty)} can't be null or white spaces or empty.");

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Value of {nameof(ItemProperty)} can't be null or white spaces or empty.");

        Name = name;
        Value = value;
    }
}
