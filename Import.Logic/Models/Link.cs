using Import.Logic.Abstractions;

namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет связь внутреннего и внешнего продукта.
/// </summary>
public sealed class Link : IEquatable<Link>, IKeyable<ExternalID>
{
    public Link(InternalID internalID, ExternalID externalID)
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

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Link link && Equals(link);

    /// <inheritdoc/>
    public bool Equals(Link? other) =>
        other is not null && other.InternalID == InternalID && other.ExternalID == ExternalID;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(InternalID, ExternalID);
}