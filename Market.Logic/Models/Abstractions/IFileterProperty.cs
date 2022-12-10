namespace Market.Logic.Models.Abstractions;

/// <summary xml:lang="ru">
/// Свойство для фильтра товаров.
/// </summary>
public interface IFileterProperty : IEquatable<IFileterProperty>
{
    /// <summary xml:lang="ru">
    /// Свойтво товара.
    /// </summary>
    public ItemProperty Property { get; }

    /// <summary xml:lang="ru">
    /// Все значения свойтва.
    /// </summary>
    public IReadOnlyDictionary<string, IFilterValue> Values { get; }

    /// <summary xml:lang="ru">
    /// Добавление нового значения к свойтву товара.
    /// </summary>
    /// <param name="value" xml:lang="ru">Значение свойтва товара.</param>
    public void AddValue(IFilterValue value);
}
