using General.Logic.Queries;
using General.Transport;

using Microsoft.Extensions.Logging;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Обработчик команд, получаемых по HTTP.
/// </summary>
public sealed class APIQueryHandler : IAPIQueryHandler
{
    private readonly ILogger<APIQueryHandler> _logger;
    private readonly IQueryFactory _commandFactory;
    private readonly IDeserializer<string, QueryParametersBase> _deserializer;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="APIQueryHandler"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="commandFactory" xml:lang = "ru">
    /// Фабрика команд.
    /// </param>
    /// <param name="deserializer" xml:lang = "ru">
    /// Десериализатор параметров команды.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из аргументов оказался <see langword="null"/>.
    /// </exception>
    public APIQueryHandler(
        ILogger<APIQueryHandler> logger,
        IQueryFactory commandFactory,
        IDeserializer<string, QueryParametersBase> deserializer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
    }

    /// <inheritdoc/>
    public async Task<IQueryResult> HandleAsync(string request, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(request))
            throw new ArgumentException(
                "Request can't be null, empty or has only whitespaces",
                nameof(request));

        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Processing new request");

        var parameters = _deserializer.Deserialize(request);

        var command = _commandFactory.Create(parameters);

        var result = await command.ExecuteAsync()
            .ConfigureAwait(false);

        _logger.LogInformation(
            "Сommand {Id} processed with success: {Result}",
            result.Id,
            result.Result);

        return result;
    }
}
