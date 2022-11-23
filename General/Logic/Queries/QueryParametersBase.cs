using General.Logic.Executables;

namespace General.Logic.Queries;

/// <summary xml:lang = "ru">
/// Базовый тип для параметров запросов.
/// </summary>
public abstract class QueryParametersBase : ExecutableParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="QueryParametersBase"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда <paramref name="id"/> оказался <see langword="null"/>.
    /// </exception>
    protected QueryParametersBase(ExecutableID id) : base(id)
    {

    }
}
