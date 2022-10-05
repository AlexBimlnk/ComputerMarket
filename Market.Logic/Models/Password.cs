namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет пароль пользователя.
/// </summary>
public class Password
{
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

        Value = value;
    }
}
