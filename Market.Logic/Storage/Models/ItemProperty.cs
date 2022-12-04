using PropertyDataTypeId = Market.Logic.Models.PropertyDataType;

namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель свойства товара, используемая хранилищем.
/// </summary>
public class ItemProperty
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
    /// Тип данных свойства.
    /// </summary>
    public PropertyDataTypeId PropertyDataTypeId { get; set; }

    /// <summary xml:lang = "ru">
    /// Тип данных используемый свойством.
    /// </summary>
    public virtual PropertyDataType DataType { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Группа к которой относится свойство.
    /// </summary>
    public virtual PropertyGroup? Group { get; set; }

    /// <summary xml:lang = "ru">
    /// Типы, к которым относится свойство.
    /// </summary>
    public virtual ICollection<ItemType> Types { get; set; } = new HashSet<ItemType>();
}
