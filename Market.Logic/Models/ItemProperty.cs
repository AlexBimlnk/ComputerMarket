using General.Models;

using Market.Logic.Storage.Models;
using Microsoft.Extensions.Logging;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Свойтсво товара.
/// </summary>
public sealed record class ItemProperty : IKeyable<ID>
{
    private string? _value = null!;

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

        if (!Enum.IsDefined(typeof(PropertyDataType), (int)dataType))
            throw new ArgumentException($"Enum value {nameof(dataType)} not match with {nameof(PropertyDataType)}.");

        Key = id;
        Group = group ?? throw new ArgumentNullException(nameof(group));
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
    public string? Value 
    {
        get => _value;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Value of {nameof(ItemProperty)} can't be null or white spaces or empty.");

            _value = value;
        }
    }

    /// <summary xml:lang = "ru">
    /// Группа свойства.
    /// </summary>
    public PropertyGroup Group { get; }

    /// <summary xml:lang = "ru">
    /// Используется ли при фильтрации.
    /// </summary>
    public bool IsFilterable { get; }

    /// <summary xml:lang = "ru">
    /// Ключ свойства.
    /// </summary>
    public ID Key { get; }

    /// <summary xml:lang = "ru">
    /// Тип данных, который хранится в свойстве.
    /// </summary>
    public PropertyDataType ProperyDataType { get; }
}
