using Market.Logic.Models.Abstractions;

namespace Market.Logic.Models;

/// <summary xml:lang="ru">
/// Фильтр для каталога товаров.
/// </summary>
public sealed class CatalogFilter : ICatalogFilter
{
    /// <inheritdoc/>
    public string? SearchString { get; set; }

    /// <inheritdoc/>
    public ItemType? SelectedType { get; set; }

    /// <inheritdoc/>
    public ISet<(ID, string)> PropertiesWithValues { get; } = new HashSet<(ID, string)>();
}
