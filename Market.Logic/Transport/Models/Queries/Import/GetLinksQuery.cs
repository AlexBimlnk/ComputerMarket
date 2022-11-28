using Market.Logic.Queries;

namespace Market.Logic.Transport.Models.Queries.Import;

/// <summary xml:lang = "ru">
/// Транспортная модель запроса для получения всех связей.
/// </summary>
public sealed class GetLinksQuery : QueryBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="GetLinksQuery"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип запроса.
    /// </param>
    public GetLinksQuery(QueryType type, string id) : base(type, id) { }

    public static GetLinksQuery ToModel(Logic.Queries.Import.GetLinksQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return new GetLinksQuery(query.Type, query.Id.Value);
    }
}