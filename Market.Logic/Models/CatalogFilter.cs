using Market.Logic.Models.Abstractions;

namespace Market.Logic.Models;

/// <summary xml:lang="ru">
/// Фильтр для каталога товаров.
/// </summary>
public sealed class CatalogFilter : ICatalogFilter
{
    public CatalogFilter(
        string? searchString = null, 
        int? typeId = null,
        IReadOnlySet<(ID, string)>? values = null) 
    { 
        SearchString = searchString;
        SelectedTypeId= typeId;
        PropertiesWithValues = values ?? new HashSet<(ID, string)>();
    }

    /// <inheritdoc/>
    public string? SearchString { get; }

    /// <inheritdoc/>
    public int? SelectedTypeId { get; }

    /// <inheritdoc/>
    public IReadOnlySet<(ID, string)> PropertiesWithValues { get; }
}
