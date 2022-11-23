using General.Logic.Executables;

namespace General.Logic.Queries;

/// <summary xml:lang = "ru">
/// Описывает фабрику запросов.
/// </summary>
public interface IQueryFactory : IExecutableFactory<IQuery, QueryParametersBase, IQueryResult>
{
}
