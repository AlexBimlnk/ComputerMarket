using System.Text.RegularExpressions;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет пароль пользователя.
/// </summary>
public class Password
{
    public const int PASSWORD_MIN_LENGTH = 8;
    public const int PASSWORD_MAX_LENGTH = 20;

    /// <summary xml:lang = "ru">
    /// Значение.
    /// </summary>
    public string Value { get; private set; }

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="Password"/>.
    /// </summary>
    /// <param name="value" xml:lang = "ru">Пароль пользователя.</param>
    /// <exception cref="ArgumentException" xml:lang = "ru">Если <paramref name="value"/> имеет неверный формат.</exception>
    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Password can't be null or empty or contains only whitespaces", nameof(value));
        
        if (value.Length < PASSWORD_MIN_LENGTH || value.Length > PASSWORD_MAX_LENGTH)
            throw new ArgumentException($"Password must have length between {PASSWORD_MIN_LENGTH} and {PASSWORD_MAX_LENGTH}", nameof(value));

        if (!Regex.IsMatch(value, AuthenticationData.ONLY_LETTERS_AND_NUMBERS_PATTERN))
            throw new ArgumentException($"Password must contains only letters and numbers", nameof(value));

        Value = value;
    }
}
