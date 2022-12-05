using PropertyDataTypeId = Market.Logic.Models.PropertyDataType;

namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Тип данных свойства.
/// </summary>
public class PropertyDataType
{
    /// <summary xml:lang = "ru">
    /// Индетификатор свойства.
    /// </summary>
    public PropertyDataTypeId PropertyDataTypeId { get; set; }

    /// <summary xml:lang = "ru">
    /// Название свойства.
    /// </summary>
    public string Name { get; set; } = default!;
}
