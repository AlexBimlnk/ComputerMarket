namespace Market.Logic.Models.Abstractions;

/// <summary xml:lang="ru">
/// Фильтр для каталога товаров.
/// </summary>
public interface ICatalogFilter
{
    /// <summary xml:lang="ru">
    /// Введенная строка для поиска товаров по названию.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary xml:lang="ru">
    /// Выбранный тип товаров.
    /// </summary>
    public ItemType? SelectedType { get; set; }

    /// <summary xml:lang="ru">
    /// Выбранный свойства товаров.
    /// </summary>
    public ISet<(ID, string)> PropertiesWithValues { get; }
}
