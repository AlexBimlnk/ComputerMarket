using General.Logic.Commands;

namespace General.Logic.Queries;

/// <summary xml:lang = "ru">
/// Описывает обработчика запросов, получаемых по HTTP.
/// </summary>
public interface IAPIQueryHandler : IAPIExecutableHandler<IQueryResult>
{
}
