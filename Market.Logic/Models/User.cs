using General.Models;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Пользователь системы.
/// </summary>
public class User : IKeyable<InternalID>
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="User"/>.
    /// </summary>
    /// <param name="id">Индетификатор пользователя.</param>
    /// <param name="authenticationData" xml:lang = "ru">Логин пользователя.</param>
    /// <param name="type" xml:lang = "ru">Тип пользователя.</param>
    /// <exception cref="ArgumentException">Если <paramref name="login"/> или  <paramref name="email"/> имеют неверный формат или <paramref name="type"/> - значение.</exception>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">Если <paramref name="password"/> равен <see langword="null"/>.</exception>
    public User(InternalID id, AuthenticationData authenticationData, UserType type)
    {
        AuthenticationData = authenticationData ?? throw new ArgumentNullException(nameof(authenticationData));

        if (!Enum.IsDefined(typeof(UserType), type))
            throw new ArgumentException($"Given user type is not mathc with enum values");

        Type = type;
        Key = id;
    }

    /// <summary xml:lang = "ru">
    /// Тип пользователя.
    /// </summary>
    public UserType Type { get; private set; }

    /// <inheritdoc/>
    public InternalID Key { get; }

    /// <summary xml:lang = "ru">
    /// Данные для ауентификации пользователя.
    /// </summary>
    public AuthenticationData AuthenticationData { get; }
}