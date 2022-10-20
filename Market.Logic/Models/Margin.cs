namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет маржу на поставщика.
/// </summary>
public struct Margin
{
    private const decimal MARGINM_MAX_VALUE = 1000;

    /// <summary xml:lang = "ru">
    /// Не поддерживаемый конструктор маржи.
    /// </summary>
    /// <exception cref="NotSupportedException"/>
    public Margin()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Создает экземпляр типа <see cref="Margin"/>.
    /// </summary>
    /// <param name="value">Входное значение маржи.</param>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="value"/> выходит за границы [1; 1000]</exception>
    public Margin(decimal value)
    {
        if (value < 1m || MARGINM_MAX_VALUE < value)
            throw new ArgumentOutOfRangeException(nameof(value));
        Value = value;
    }

    /// <summary xml:lang = "ru">
    /// Значение маржи.
    /// </summary>
    public decimal Value { get; }
}
