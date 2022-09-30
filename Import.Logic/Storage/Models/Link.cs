namespace Import.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель связи, используемая хранилищем.
/// </summary>
public sealed class Link
{
    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор продукта.
    /// </summary>
    public long InternalId { get; set; }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </summary>
    public long ExternalId { get; set; }

    /// <summary xml:lang = "ru">
    /// Идентификатор поставщика.
    /// </summary>
    public short ProviderId { get; set; }

    /// <summary xml:lang = "ru">
    /// Поставщик.
    /// </summary>
    public Provider Provider { get; set; } = null!;
}
