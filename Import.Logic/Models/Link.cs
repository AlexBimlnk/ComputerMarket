using General.Models;

namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет связь внутреннего и внешнего продукта.
/// </summary>
public sealed record class Link : IEquatable<Link>, IKeyable<ExternalID>
{
    public Link(ExternalID externalID, InternalID internalID = default)
    {
        InternalID = internalID;
        ExternalID = externalID;
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    public InternalID InternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    public ExternalID ExternalID { get; }

    /// <inheritdoc/>
    public ExternalID Key => ExternalID;
}