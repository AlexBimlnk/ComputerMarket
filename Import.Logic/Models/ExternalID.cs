namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Внешний идентификатор продукта.
/// </summary>
/// <param name="Value" xml:lang = "ru">
/// Значение.
/// </param>
/// <param name="Provider" xml:lang = "ru">
/// Поставщик продукта.
/// </param>
public record struct ExternalID(long Value, Provider Provider);