namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет связь внутреннего и внешнего продукта.
/// </summary>
public sealed record class Link
{
    public Link(ID internalID, ID externalID)
    {
        InternalID = internalID;
        ExternalID = externalID;
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    public ID InternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    public ID ExternalID { get; }
}