namespace Import.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель истории, используемая хранилищем.
/// </summary>
public sealed class History
{
    /// <summary xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </summary>
    public int ExternalId { get; set; }

    /// <summary xml:lang = "ru">
    /// Идентификатор поставщика.
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary xml:lang = "ru">
    /// Название продукта.
    /// </summary>
    public string ProductName { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Описание продукта.
    /// </summary>
    public string? ProductDescription { get; set; }
}
