namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель представителя поставщика, используемая хранилищем.
/// </summary>
public class ProviderAgent
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
    public virtual Provider Provider { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Пользователь.
    /// </summary>
    public virtual User User { get; set; } = null!;
}
