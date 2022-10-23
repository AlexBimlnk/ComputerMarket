namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель представителя поставщика, используемая хранилищем.
/// </summary>
public sealed class ProviderAgent
{
    /// <summary xml:lang = "ru">
    /// Индетификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary xml:lang = "ru">
    /// Индетификатор поставщика.
    /// </summary>
    public long ProviderId { get; set; }

    /// <summary xml:lang = "ru">
    /// Поставщик.
    /// </summary>
    public Provider Provider { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Пользователь.
    /// </summary>
    public User User { get; set; } = null!;
}
