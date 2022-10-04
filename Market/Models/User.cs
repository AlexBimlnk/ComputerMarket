namespace Market.Models;

/// <summary xml:lang = "ru">
/// Пользователь системы.
/// </summary>
public class User
{
    /// <summary>
    /// Создает экземпляр типа <see cref="User"/>.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <param name="type">Тип пользователя.</param>
    /// <exception cref="ArgumentNullException">Если любой из параметров равен <see langword="null"/>.</exception>
    public User(string login, string password, UserType type)
    {
        Login = login ?? throw new ArgumentNullException(nameof(login));
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
    public string Password { get; set; }

    /// <summary xml:lang = "ru">
    /// Тип пользователя.
    /// </summary>
    public UserType Type { get; set; }

}