using General.Logic.Commands;
using General.Logic.Queries;

using Microsoft.AspNetCore.Mvc;

namespace Import.Controllers;

/// <summary xml:lang = "ru">
/// ���������� ������.
/// </summary>
[ApiController]
[Route("import/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ILogger<CommandsController> _logger;
    private readonly IAPICommandHandler _commandHandler;
    private readonly IAPIQueryHandler _queryHandler;

    /// <summary xml:lang = "ru">
    /// ������ ����� ��������� ���� <see cref="CommandsController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// ������.
    /// </param>
    /// <param name="commandHandler" xml:lang = "ru">
    /// ���������� ������.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// ����� ����� �� ������� �������� <see langword="null"/>.
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
    /// ��������� ������ �� ���������� �������.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// ��������� ���������� ������� ���� <see cref="ICommandResult"/>.
    /// </returns>
    [HttpPost]
    public async Task<ICommandResult> ExecuteCommandAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request");

        return await _commandHandler.HandleAsync(body);
    }

    /// <summary xml:lang = "ru">
    /// ��������� ������ �� ���������� �������.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// ��������� ���������� ������� ���� <see cref="IQueryResult"/>.
    /// </returns>
    [HttpGet]
    public async Task<IQueryResult> ExecuteQueryAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request");

        return await _queryHandler.HandleAsync(body);
    }
}