﻿namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель свойства товара, используемая хранилищем.
/// </summary>
public sealed class ItemProperty
{
    /// <summary xml:lang = "ru">
    /// Индетификатор свойства.
    /// </summary>
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название свойства.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Индетификатор группы.
    /// </summary>
    public int? GroupId { get; set; }

    /// <summary xml:lang = "ru">
    /// Используется ли для фильтра.
    /// </summary>
    public bool IsFilterable { get; set; }

    /// <summary xml:lang = "ru">
    /// Тип данных используемый свойством.
    /// </summary>
    public string PropertyDataType { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Группа к которой относится свойство.
    /// </summary>
    public PropertyGroup? Group { get; set; }

    /// <summary xml:lang = "ru">
    /// Значения этого свойства у кажого товара.
    /// </summary>
    public ICollection<ItemDescription> ItemDescriptions { get; set; } = new HashSet<ItemDescription>();

    /// <summary xml:lang = "ru">
    /// Товары, которые имеют данное свойство.
    /// </summary>
    public ICollection<Item> Items { get; set; } = new HashSet<Item>();

    /// <summary xml:lang = "ru">
    /// Типы, к которым относится свойство.
    /// </summary>
    public ICollection<ItemType> Types { get; set; } = new HashSet<ItemType>();
}
