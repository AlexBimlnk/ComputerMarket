namespace Market.Logic.Models.Abstractions;

/// <summary xml:lang="ru">
/// Каталог твоаров.
/// </summary>
public interface ICatalog
{
    /// <summary>
    /// Поис товаров по введенному имени.
    /// </summary>
    /// <param name="name">Введенное название товара.</param>
    /// <returns>Список найденных продутов.</returns>
    public IProducts Search(string name);

    public IEnumerable<ItemType> Type { get; }

    public IProducts ProductOfType(ItemType type);

    public IEnumerable<IFileterProperty> Properties(IProducts products); 
}
