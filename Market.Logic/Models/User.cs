using System.Text.RegularExpressions;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Пользователь системы.
/// </summary>
public class User
{
    private const string EMAIL_PATTERN = @"^\w+@\w+\.\w+$";

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="User"/>.
    /// </summary>
    /// <param name="login" xml:lang = "ru">Логин пользователя.</param>
    /// <param name="password" xml:lang = "ru">Пароль пользователя.</param>
    /// <param name="type" xml:lang = "ru">Тип пользователя.</param>
    /// <param name="email" xml:lang = "ru">Email пользователя.</param>
    /// <exception cref="ArgumentException">Если <paramref name="login"/> или  <paramref name="email"/> имеют неверный формат или <paramref name="type"/> - значение.</exception>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">Если <paramref name="password"/> равен <see langword="null"/>.</exception>
    public User(string login, Password password, string email, UserType type)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException($"Login can't be null or empty or contains only whitespaces", nameof(login));
        Login = login;

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"Email can't be null or empty or contains only whitespaces", nameof(email));
        if (!Regex.IsMatch(email, EMAIL_PATTERN))
            throw new ArgumentException($"Given email is not match with email pattern", nameof(email));
        Email = email;

        Password = password ?? throw new ArgumentNullException(nameof(login));

        if (!Enum.IsDefined(typeof(UserType), type))
            throw new ArgumentException($"Given user type is not mathc with enum values");

        Type = type;
    }

    /// <summary xml:lang = "ru">
    /// Логин пользователя.
    /// </summary>
    public string Login { get; private set; }

    /// <summary xml:lang = "ru">
    /// Пароль пользователя.
    /// </summary>
    public Password Password { get; private set; }

    /// <summary xml:lang = "ru">
    /// Email пользователя.
    /// </summary>
    public string Email { get; private set; }

    /// <summary xml:lang = "ru">
    /// Тип пользователя.
    /// </summary>
    public UserType Type { get; private set; }

}