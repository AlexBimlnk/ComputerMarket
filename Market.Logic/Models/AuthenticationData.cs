using System.Text.RegularExpressions;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
///  Данные для ауентификации пользователя в системе.
/// </summary>
public sealed class AuthenticationData
{
    /// <summary xml:lang = "ru">
    /// Шабон для электронной почты полльзователя.
    /// </summary>
    public const string EMAIL_PATTERN = @"^(?=.{3,256}$)\w+@\w+\.\w+$";

    /// <summary xml:lang = "ru">
    /// Шаблон для логина пользователя.
    /// </summary>
    public const string LOGIN_PATTERN = @"^[a-zA-Z0-9_.-]{6,20}$";

    private string? _login;

    /// <summary xml:lang = "ru">
    /// Создаёт экземпляр класса <see cref="AuthenticationData"/>.
    /// </summary>
    /// <param name="email" xml:lang = "ru">
    /// Электронная почта пользователя.
    /// </param>
    /// <param name="password" xml:lang = "ru">
    /// Пароль пользователя.
    /// </param>
    /// <param name="login" xml:lang = "ru">
    /// Логин пользователя.
    /// </param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="email"/> или <paramref name="login"/> не соотвествуют формату.
    /// </exception>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="password"/> - <see langword="null"/>/
    /// </exception>
    public AuthenticationData(string email, Password password, string? login = null)
    {
        if (login is not null)
        {
            Login = login;
        }

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"Email can't be null or empty or contains only whitespaces", nameof(email));
        if (!Regex.IsMatch(email, EMAIL_PATTERN))
            throw new ArgumentException($"Given email is not match with email pattern", nameof(email));

        Email = email;

        Password = password ?? throw new ArgumentNullException(nameof(password));
    }

    /// <summary xml:lang = "ru">
    /// Логин пользователя.
    /// </summary>
    public string Login
    {
        get => _login ?? throw new InvalidOperationException("Login not set.");

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Login can't be null or empty or contains only whitespaces", nameof(value));

            if (!Regex.IsMatch(value, LOGIN_PATTERN))
                throw new ArgumentException($"Login must contains only letters and numbers", nameof(value));

            _login = value;
        }

    }

    /// <summary xml:lang = "ru">
    /// Email.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public Password Password { get; }
}
