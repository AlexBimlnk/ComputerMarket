namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет связь внутреннего и внешнего продукта.
/// </summary>
public sealed class Link : IEquatable<Link>
{
    public Link(InternalID internalID, ExternalID externalID)
    {
        InternalID = internalID;
        ExternalID = externalID;
    }

    public InternalID InternalID { get; }
    public ExternalID ExternalID { get; }

    public override bool Equals(object? obj) => obj is Link link && Equals(link);

    public bool Equals(Link? other) => 
        other is not null && other.InternalID == InternalID && other.ExternalID == ExternalID;

    public override int GetHashCode() => HashCode.Combine(InternalID, ExternalID);
}