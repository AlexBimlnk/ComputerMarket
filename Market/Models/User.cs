namespace Market.Models;

/// <summary xml:lang = "ru">
/// Пользователь системы.
/// </summary>
public class User
{
    /// <summary xml:lang = "ru">
    /// Идентификатор пользователя.
    /// </summary>
    public int Id { get; set; }

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