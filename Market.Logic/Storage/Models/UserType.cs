namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортаня модель типа пользователя, используемая хранилищем.
/// </summary>
public class UserType
{
    /// <summary xml:lang = "ru">
    /// Индетифкатор типа пользователя.
    /// </summary>
    public short Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название типа пользователя.
    /// </summary>
    public string Name { get; set; } = null!;
}