namespace Import.Logic.Storage.Models;

/// <summary>
/// Транспортная модель поставщика, используемая хранилищем.
/// </summary>
public sealed class Provider
{
    /// <summary>
    /// Идентификатор поставщика.
    /// </summary>
    public short Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Имя поставщика.
    /// </summary>
    public string Name { get; set; } = null!;
}
