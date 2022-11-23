using General.Logic.Executables;
using General.Logic.Queries;

using Import.Logic.Queries;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Фабрика запросов.
/// </summary>
public sealed class QueryFactory : IQueryFactory
{
    private readonly Func<GetLinksQueryParameters, IQuery> _getLinkskCommandFactory;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CommandFactory"/>.
    /// </summary>
    /// <param name="getLinkskCommandFactory" xml:lang = "ru">
    /// Делегат, создающий на основе <see cref="ExecutableID"/> и <see cref="GetLinksQueryParameters"/>
    /// запрос типа <see cref="IQuery"/>.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из аргументов оказался <see langword="null"/>.
    /// </exception>
    public QueryFactory(Func<GetLinksQueryParameters, IQuery> getLinkskCommandFactory)
    {
        _getLinkskCommandFactory = getLinkskCommandFactory ?? throw new ArgumentNullException(nameof(getLinkskCommandFactory));
    }

    /// <inheritdoc/>
    public IQuery Create(QueryParametersBase parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        return parameters switch
        {
            GetLinksQueryParameters getLinksCommandParameters =>
                _getLinkskCommandFactory(getLinksCommandParameters),

            _ => throw new ArgumentException(
              $"The command parameters type is unknown {parameters.GetType().Name}",
              nameof(parameters))
        };
    }
}
