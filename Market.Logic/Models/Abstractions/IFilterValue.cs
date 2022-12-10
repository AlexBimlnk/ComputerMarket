namespace Market.Logic.Models.Abstractions;

/// <summary xml:lang="ru">
/// Значение свойства товарва.
/// </summary>
public interface IFilterValue : IEquatable<IFilterValue>
{
    /// <summary xml:lang="ru">
    /// Значение свойства.
    /// </summary>
    public string Value { get; }

    /// <summary xml:lang="ru">
    /// Колличество раз сколько значение свойства встречалось у каждого товара.
    /// </summary>
    public int Count { get; set; }

    /// <summary xml:lang="ru">
    /// Было ли выбран значение свойтва.
    /// </summary>
    public bool Selected { get; set; }
}
