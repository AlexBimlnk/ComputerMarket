﻿using Import.Logic.Abstractions;
using Import.Logic.Abstractions.Commands;
using Import.Logic.Commands;

using Microsoft.Extensions.Logging;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Обработчик команд, получаемых по HTTP.
/// </summary>
public sealed class HTTPCommandHandler : IHTTPCommandHandler
{
    private readonly ILogger<HTTPCommandHandler> _logger;
    private readonly ICommandFactory _commandFactory;
    private readonly IDeserializer<string, CommandParametersBase> _deserializer;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="HTTPCommandHandler"/>.
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
    public HTTPCommandHandler(
        ILogger<HTTPCommandHandler> logger,
        ICommandFactory commandFactory,
        IDeserializer<string, CommandParametersBase> deserializer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
    }

    /// <inheritdoc/>
    public async Task<CommandResult> HandleAsync(string request)
    {
        if (string.IsNullOrWhiteSpace(request))
            throw new ArgumentException(
                "Request can't be null, empty or has only whitespaces",
                nameof(request));

        _logger.LogDebug("Processing new request");

        var parameters = _deserializer.Deserialize(request);

        var command = _commandFactory.Create(parameters);

        var result = await command.ExecuteAsync();

        _logger.LogInformation(
            "Сommand {Id} processed with success: {Result}", 
            result.Id, 
            result.IsSuccess);

        return result;
    }
}