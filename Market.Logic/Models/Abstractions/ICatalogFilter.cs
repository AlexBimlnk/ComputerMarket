namespace Market.Logic.Models.Abstractions;

/// <summary xml:lang="ru">
/// Фильтр для каталога товаров.
/// </summary>
public interface ICatalogFilter
{
    /// <summary xml:lang="ru">
    /// Введенная строка для поиска товаров по названию.
    /// </summary>
    public string? SearchString { get; }

    /// <summary xml:lang="ru">
    /// Выбранный тип товаров.
    /// </summary>
    public int? SelectedTypeId { get; }

    /// <summary xml:lang="ru">
    /// Выбранный свойства товаров.
    /// </summary>
    public IReadOnlySet<(ID, string)> PropertiesWithValues { get; }
}
