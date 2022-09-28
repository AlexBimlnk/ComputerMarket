namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет связь внутреннего и внешнего продукта.
/// </summary>
/// <param name="InternalID" xml:lang = "ru">
/// Внутренний идентификатор.
/// </param>
/// <param name="ExternalID" xml:lang = "ru">
/// Внешний идентификатор.
/// </param>
public sealed record class Link(InternalID InternalID, ExternalID ExternalID);