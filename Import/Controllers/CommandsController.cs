using General.Logic.Commands;
using General.Logic.Queries;

using Microsoft.AspNetCore.Mvc;

namespace Import.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер команд.
/// </summary>
[ApiController]
[Route("import/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ILogger<CommandsController> _logger;
    private readonly IAPICommandHandler _commandHandler;
    private readonly IAPIQueryHandler _queryHandler;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="CommandsController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="commandHandler" xml:lang = "ru">
    /// Обработчик команд.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из входных оказался <see langword="null"/>.
    /// </exception>
    public CommandsController(
        ILogger<CommandsController> logger,
        IAPICommandHandler commandHandler,
        IAPIQueryHandler queryHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        _queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
    }

    private async Task<string> ReadRequestBodyAsync()
    {
        using var reader = new StreamReader(Request.Body);
        return await reader.ReadToEndAsync();
    }

    /// <summary xml:lang = "ru">
    /// Принимает запрос на выполнение команды.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Результат выполнения команды типа <see cref="ICommandResult"/>.
    /// </returns>
    [HttpPost]
    public async Task<ICommandResult> ExecuteCommandAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request");

        return await _commandHandler.HandleAsync(body);
    }

    /// <summary xml:lang = "ru">
    /// Принимает запрос на выполнение команды.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Результат выполнения команды типа <see cref="IQueryResult"/>.
    /// </returns>
    [HttpGet]
    public async Task<IQueryResult> ExecuteQueryAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request");

        return await _queryHandler.HandleAsync(body);
    }
}