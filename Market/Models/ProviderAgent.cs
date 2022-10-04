namespace Market.Models;

/// <summary xml:lang = "ru">
/// Представитель поставщика.
/// </summary>
public class ProviderAgent
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="ProviderAgent"/>.
    /// </summary>
    /// <param name="agent" xml:lang = "ru">Пользователь.</param>
    /// <param name="provider" xml:lang = "ru">Поставщик.</param>
    /// <exception cref="ArgumentException" xml:lang = "ru">Когда тип пользователя не <see cref="UserType.Agent"/>.</exception>
    public ProviderAgent(User agent, Provider provider)
    {
        Agent = agent switch
        {
            { Type: UserType.Agent } => agent,
            null => throw new ArgumentNullException(nameof(agent)),
            _ => throw new ArgumentException("Given user is not agent", nameof(agent))
        };

        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <summary xml:lang = "ru">
    /// Представитель.
    /// </summary>
    public User Agent { get; private set; }

    /// <summary xml:lang = "ru">
    /// Поставщик.
    /// </summary>
    public Provider Provider { get; private set; }
}
