namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// История о получении продукта.
/// </summary>
public sealed class History
{
    /// <summary xml:lang = "ru">
    /// Создаёт новый объект типа <see cref="History"/>.
    /// </summary>
    /// <param name="externalId" xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </param>
    /// <param name="productMetadata" xml:lang = "ru">
    /// Методанные продукта.
    /// </param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если имя поставщика отсутствует.
    /// </exception>
    public History(
        ExternalID externalId,
        string? productMetadata = null)
    {
        ExternalId = externalId;

        ProductMetadata = productMetadata;
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </summary>
    public ExternalID ExternalId { get; }

    /// <summary xml:lang = "ru">
    /// Методанные продукта.
    /// </summary>
    public string? ProductMetadata { get; }

    /// <inheritdoc/>
    public override string ToString() =>
        $"External id: {ExternalId}, Metadata: {ProductMetadata}";
}
