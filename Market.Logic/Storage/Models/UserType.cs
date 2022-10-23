namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортаня модель типа пользователя, используемая хранилищем.
/// </summary>
public sealed class UserType
{
    /// <summary xml:lang = "ru">
    /// Индетифкатор типа пользователя.
    /// </summary>
    public short Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название типа пользователя.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Пользователи относящиеся к данному типу.
    /// </summary>
    public ICollection<User> Users { get; set; } = new HashSet<User>();
}