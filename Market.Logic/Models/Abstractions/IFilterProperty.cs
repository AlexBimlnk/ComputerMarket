namespace Market.Logic.Models.Abstractions;

/// <summary xml:lang="ru">
/// Свойство для фильтра товаров.
/// </summary>
public interface IFilterProperty : IEquatable<IFilterProperty>
{
    /// <summary xml:lang="ru">
    /// Свойство товара.
    /// </summary>
    public ItemProperty Property { get; }

    /// <summary xml:lang="ru">
    /// Все значения свойтва.
    /// </summary>
    public IReadOnlySet<IFilterValue> Values { get; }

    /// <summary xml:lang="ru">
    /// Добавление нового значения к свойству товара.
    /// </summary>
    /// <param name="value" xml:lang="ru">Значение свойтва товара.</param>
    public void AddValue(IFilterValue value);
}
