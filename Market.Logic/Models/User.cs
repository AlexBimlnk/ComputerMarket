namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Пользователь системы.
/// </summary>
public class User
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="User"/>.
    /// </summary>
    /// <param name="login" xml:lang = "ru">Логин пользователя.</param>
    /// <param name="password" xml:lang = "ru">Пароль пользователя.</param>
    /// <param name="type" xml:lang = "ru">Тип пользователя.</param>
    /// <exception cref="ArgumentException">Если <paramref name="login"/> имеет неверный формат.</exception>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">Если <paramref name="password"/> равен <see langword="null"/>.</exception>
    public User(string login, Password password, UserType type)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException($"Login can't be null or empty or contains only whitespaces", nameof(login));
        Login = login;

        Password = password ?? throw new ArgumentNullException(nameof(login));

        Type = type;
    }

    /// <summary xml:lang = "ru">
    /// Логин пользователя.
    /// </summary>
    public string Login { get; set; }

    /// <summary xml:lang = "ru">
    /// Пароль пользователя.
    /// </summary>
    public Password Password { get; set; }

    /// <summary xml:lang = "ru">
    /// Тип пользователя.
    /// </summary>
    public UserType Type { get; set; }

}