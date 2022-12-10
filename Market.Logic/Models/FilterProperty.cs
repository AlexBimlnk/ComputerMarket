using Market.Logic.Models.Abstractions;

namespace Market.Logic.Models;

/// <summary xml:lang="ru">
/// Свойтва для фильтра по свойствам товаров.
/// </summary>
public sealed class FilterProperty : IFileterProperty
{
    private readonly Dictionary<string, IFilterValue> _values;

    /// <summary xml:lang="ru">
    /// Создаёт экземпляра класса <see cref="FilterProperty"/>.
    /// </summary>
    /// <param name="property" xml:lang="ru">Свойство товара.</param>
    /// <exception cref="ArgumentNullException" xml:lang="ru">Если <paramref name="property"/> - <see langword="null"/>.</exception>
    public FilterProperty(ItemProperty property)
    {
        Property = property ?? throw new ArgumentNullException(nameof(property));

        _values = new Dictionary<string, IFilterValue>();
    }

    /// <inheritdoc/>
    public ItemProperty Property { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, IFilterValue> Values => _values;

    /// <inheritdoc/>
    public void AddValue(IFilterValue value)
    {
        if (!_values.ContainsKey(value.Value))
        {
            value.Count = 1;
            value.Selected = false;
            _values.Add(value.Value, value);
            return;
        }

        _values[value.Value].Count++;
    }
    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Property);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is IFileterProperty property && Equals(property);

    /// <inheritdoc/>
    public bool Equals(IFileterProperty? other) => Property.Equals(other?.Property.Key);
}
