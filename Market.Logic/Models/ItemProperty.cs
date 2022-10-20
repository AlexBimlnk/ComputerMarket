namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Свойтсво товара.
/// </summary>
public sealed record class ItemProperty
{
    /// <summary xml:lang = "ru">
    /// Название свойства.
    /// </summary>
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// Значения свойства.
    /// </summary>
    public string Value { get; }

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="ItemProperty"/>.
    /// </summary>
    /// <param name="name" xml:lang = "ru">Название свойства.</param>
    /// <param name="value" xml:lang = "ru">Значение свойства.</param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
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
