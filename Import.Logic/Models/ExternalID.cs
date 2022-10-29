namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Внешний идентификатор продукта.
/// </summary>
public record struct ExternalID
{
    /// <summary xml:lang = "ru">
    /// Неподдерживаемый конструктор.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public ExternalID()
    {
        throw new NotSupportedException();
    }

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="ExternalID"/>.
    /// </summary>
    /// <param name="value" xml:lang = "ru">
    /// Значение идентифактора.
    /// </param>
    /// <param name="provider" xml:lang = "ru">
    /// Поставщик.
    /// </param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="provider"/> не был определен.
    /// </exception>
    public ExternalID(long value, Provider provider)
    {
        if (!Enum.IsDefined(typeof(Provider), provider))
            throw new ArgumentException("Given unknown provider.", nameof(provider));

        Provider = provider;
        Value = value;
    }

    /// <summary xml:lang = "ru">
    /// Значение.
    /// </summary>
    public long Value { get; }

    /// <summary xml:lang = "ru">
    /// Поставщик продукта.
    /// </summary>
    public Provider Provider { get; }
}