using Market.Logic.Models.Abstractions;

namespace Market.Logic.Models;

/// <summary xml:lang="ru">
/// Свойтва для фильтра по свойствам товаров.
/// </summary>
public sealed class FilterProperty : IFilterProperty
{
    private readonly HashSet<IFilterValue> _values;

    /// <summary xml:lang="ru">
    /// Создаёт экземпляра класса <see cref="FilterProperty"/>.
    /// </summary>
    /// <param name="property" xml:lang="ru">Свойство товара.</param>
    /// <exception cref="ArgumentNullException" xml:lang="ru">Если <paramref name="property"/> - <see langword="null"/>.</exception>
    public FilterProperty(ItemProperty property)
    {
        Property = property ?? throw new ArgumentNullException(nameof(property));

        _values = new HashSet<IFilterValue>();
    }

    /// <inheritdoc/>
    public ItemProperty Property { get; }

    /// <inheritdoc/>
    public IReadOnlySet<IFilterValue> Values => _values;

    /// <inheritdoc/>
    public void AddValue(IFilterValue value) =>
        _values.Add(value ?? throw new ArgumentNullException(nameof(value)));

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Property);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is IFilterProperty property && Equals(property);

    /// <inheritdoc/>
    public bool Equals(IFilterProperty? other) => Property.Equals(other?.Property.Key);
}
