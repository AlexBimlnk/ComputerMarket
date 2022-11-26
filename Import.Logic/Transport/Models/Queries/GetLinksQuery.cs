namespace Import.Logic.Transport.Models.Queries;

/// <summary xml:lang = "ru">
/// Транспортная модель запроса для получения всех связей.
/// </summary>
public sealed class GetLinksQuery : QueryBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="SetLinkCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип запроса.
    /// </param>
    public GetLinksQuery(QueryType type, string id) : base(type, id) { }
}