namespace Market.Models;

/// <summary xml:lang = "ru">
/// Виды пользователей.
/// </summary>
public enum UserType
{
    /// <summary xml:lang = "ru">
    /// Обычный пользователь системы.
    /// </summary>
    Customer = 0,

    /// <summary xml:lang = "ru">
    /// Представитель.
    /// </summary>
    Agent = 1,

    /// <summary xml:lang = "ru">
    /// Менеджер.
    /// </summary>
    Manager = 2
}