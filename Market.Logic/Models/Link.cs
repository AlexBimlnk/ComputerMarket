namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет связь внутреннего и внешнего продукта.
/// </summary>
public sealed record class Link
{
    public Link(ID internalID, ID externalID, ID providerID)
    {
        InternalID = internalID;
        ExternalID = externalID;
        ProviderID = providerID;
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    public ID InternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    public ID ExternalID { get; }

    /// <summary xml:lang = "ru">
    /// Идентификатор поставщика.
    /// </summary>
    public ID ProviderID { get; }
}