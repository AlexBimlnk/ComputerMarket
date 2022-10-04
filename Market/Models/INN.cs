using System.Text.RegularExpressions;

namespace Market.Models;

/// <summary xml:lang = "ru">
/// Представляет ИНН поставщика.
/// </summary>
public record struct INN
{
    private readonly static string s_innPattern = @"^[0-9]{10}$";

    /// <summary xml:lang = "ru">
    /// Значение.
    /// </summary>
    public string Value { get; private set; }

    /// <summary xml:lang = "ru">
    /// Не поддерживаемы контсруктор <see cref="INN"/>.
    /// </summary>
    /// <exception cref="NotSupportedException"/>
    public INN()
    {
        throw new NotSupportedException();
    }

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="INN"/>.
    /// </summary>
    /// <param name="value" xml:lang = "ru">Инн поставщика.</param>
    /// <exception cref="ArgumentException">Если <paramref name="value"/> имеент неверный формат.</exception>
    public INN(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("INN can't be null or empty or contains only whitespaces", nameof(value));

        if (!Regex.IsMatch(value, s_innPattern))
            throw new ArgumentException("Given inn not match with INN format");

        Value = value;
    }
}
