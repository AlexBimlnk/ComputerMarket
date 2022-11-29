using General.Models;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Свойтсво товара.
/// </summary>
public sealed record class ItemProperty : IKeyable<ID>
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="ItemProperty"/>.
    /// </summary>
    /// <param name="name" xml:lang = "ru">Название свойства.</param>
    /// <param name="value" xml:lang = "ru">Значение свойства.</param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="name"/> или <paramref name="value"/> - пустые, состоят из пробелов или <see langword="null"/>.
    /// </exception>
    public ItemProperty(ID id, string name, PropertyGroup group, bool isFilterable, PropertyDataType dataType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name of {nameof(ItemProperty)} can't be null or white spaces or empty.");

        Key= id;
        Group = group;
        Name = name;
        ProperyDataType = dataType;
        IsFilterable = isFilterable;
    }

    /// <summary xml:lang = "ru">
    /// Название свойства.
    /// </summary>
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// Значения свойства.
    /// </summary>
    public string? Value { get; private set; } = null;

    public void SetValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Value of {nameof(ItemProperty)} can't be null or white spaces or empty.");

        Value = value;
    }

    public PropertyGroup Group { get; }

    public bool IsFilterable { get; }

    public ID Key { get; }

    public PropertyDataType ProperyDataType { get; }
}
