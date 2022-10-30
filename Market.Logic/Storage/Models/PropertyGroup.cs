namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель группы свойств, используемая хранилищем.
/// </summary>
public class PropertyGroup
{
    /// <summary xml:lang = "ru">
    /// Индетификатор группы свойств.
    /// </summary>
    public int Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название группы.
    /// </summary>
    public string? Name { get; set; }
}
