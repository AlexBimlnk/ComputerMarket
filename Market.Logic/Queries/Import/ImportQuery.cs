using General.Logic.Executables;

namespace Market.Logic.Queries.Import;

/// <summary xml:lang = "ru">
/// Маркерный класс запросов для сервиса импорта.
/// </summary>
public abstract class ImportQuery : QueryBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="ImportQuery"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    protected ImportQuery(ExecutableID id, QueryType type)
        : base(id, type)
    {
    }
}
