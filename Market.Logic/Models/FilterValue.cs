using System.Xml.Linq;

using Market.Logic.Models.Abstractions;

namespace Market.Logic.Models;

/// <summary xml:lang="ru">
/// Значение для свойства товара.
/// </summary>
public sealed class FilterValue : IFilterValue
{
    /// <summary xml:lang="ru">
    /// Создаёт экземпляр класса <see cref="FilterValue"/>.
    /// </summary>
    /// <param name="value" xml:lang="ru">Значение свойства.</param>
    public FilterValue(ID propertyId, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Value of {nameof(value)} can't be empty or null or contains only white spaces");

        PropertyID = propertyId;
        Value = value;
        Selected = false;
    }

    /// <inheritdoc/>
    public ID PropertyID { get; }

    /// <inheritdoc/>
    public string Value { get; }

    /// <inheritdoc/>
    public bool Selected { get; set; }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Value, PropertyID);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is IFilterValue value && Equals(value);

    /// <inheritdoc/>
    public bool Equals(IFilterValue? other) => Value.Equals(other?.Value) && PropertyID.Equals(other?.PropertyID);
}
