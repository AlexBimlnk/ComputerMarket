using System.Text.RegularExpressions;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет пароль пользователя.
/// </summary>
public class Password
{
    /// <summary xml:lang = "ru">
    /// Шабон для строки пароля.
    /// </summary>
    public const string PASSWORD_PATTERN = @"^[a-zA-Z0-9_.-]{8,20}$";

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
        
        if (!Regex.IsMatch(value, PASSWORD_PATTERN))
            throw new ArgumentException($"Password must contains only letters and numbers", nameof(value));

        Value = value;
    }
}
