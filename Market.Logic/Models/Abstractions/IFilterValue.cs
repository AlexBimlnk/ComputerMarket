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
    /// Было ли выбрано значение свойтва.
    /// </summary>
    public bool Selected { get; set; }
}
