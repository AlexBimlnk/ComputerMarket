using Import.Logic.Abstractions.Commands;
using Import.Logic.Commands;

using Microsoft.AspNetCore.Mvc;

namespace Import.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер команд.
/// </summary>
[ApiController]
[Route("[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ILogger<ImportController> _logger;
    private readonly IAPICommandHandler _commandHandler;

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
        ILogger<ImportController> logger,
        IAPICommandHandler commandHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
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
    /// Результат выполнения команды типа <see cref="CommandResult"/>.
    /// </returns>
    [HttpGet]
    public async Task<CommandResult> ExecuteCommandAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request");

        return await _commandHandler.HandleAsync(body);
    }
}