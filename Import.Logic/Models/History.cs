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
    /// <param name="productName" xml:lang = "ru">
    /// Название продукта.
    /// </param>
    /// <param name="productDescription" xml:lang = "ru">
    /// Описание продукта.
    /// </param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если имя поставщика отсутствует.
    /// </exception>
    public History(
        ExternalID externalId,
        string productName, 
        string? productDescription = default)
    {
        ExternalId = externalId;

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException(nameof(productName));

        ProductName = productName ?? throw new ArgumentException(nameof(productName));
        ProductDescription = productDescription;
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </summary>
    public ExternalID ExternalId { get; }

    /// <summary xml:lang = "ru">
    /// Название продукта.
    /// </summary>
    public string ProductName { get; }

    /// <summary xml:lang = "ru">
    /// Описание продукта.
    /// </summary>
    public string? ProductDescription { get; }

    /// <inheritdoc/>
    public override string ToString() =>
        $"External id: {ExternalId}, Product name: {ProductName}, Description: {ProductDescription}";
}
