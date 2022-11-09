using System.Text.RegularExpressions;

using General.Models;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Пользователь системы.
/// </summary>
public class User : IKeyable<InternalID>, IKeyable<string>
{
    public const string EMAIL_PATTERN = @"^\w+@\w+\.\w+$";
    public const string ONLY_LETTERS_AND_NUMBERS_PATTERN = @"^[a-zA-Z0-9_.-]*$";
    public const int LOGIN_MAX_LENGTH = 20; 
    public const int LOGIN_MIN_LENGTH = 6; 
    public const int PASSWORD_MIN_LENGTH = 8; 
    public const int PASSWORD_MAX_LENGTH = 20; 
    public const int EMAIL_MAX_LENGTH = 256; 
    public const int EMAIL_MIN_LENGTH = 3; 

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="User"/>.
    /// </summary>
    /// <param name="id">Индетификатор пользователя.</param>
    /// <param name="login" xml:lang = "ru">Логин пользователя.</param>
    /// <param name="password" xml:lang = "ru">Пароль пользователя.</param>
    /// <param name="type" xml:lang = "ru">Тип пользователя.</param>
    /// <param name="email" xml:lang = "ru">Email пользователя.</param>
    /// <exception cref="ArgumentException">Если <paramref name="login"/> или  <paramref name="email"/> имеют неверный формат или <paramref name="type"/> - значение.</exception>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">Если <paramref name="password"/> равен <see langword="null"/>.</exception>
    public User(InternalID id, string login, Password password, string email, UserType type)
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
        Key = id;
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

    /// <inheritdoc/>
    public InternalID Key { get; }

    /// <inheritdoc/>
    string IKeyable<string>.Key => Email;
}