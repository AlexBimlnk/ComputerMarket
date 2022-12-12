using General.Logic;

using Market.Logic.Markers;

using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер для по обработке оповещений от внешних сервисов.
/// </summary>
[ApiController]
[Route("market/external/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly ILogger<NotificationController> _logger;
    private readonly IAPIRequestHandler<ImportMarker> _importRequestHandler;
    private readonly IAPIRequestHandler<WTMarker> _wtRequestHandler;

    /// <summary xml:lang = "ru">
    /// Создаёт экземпляр класса <see cref="NotificationController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    /// <param name="importRequestHandler" xml:lang = "ru">
    /// Обработчик оповещений от импорта.
    /// </param>
    /// <param name="wtRequestHandler" xml:lang = "ru">
    /// Обработчик оповещений от WT.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если один из параметров - <see langword="null"/>.
    /// </exception>
    public NotificationController(
        ILogger<NotificationController> logger,
        IAPIRequestHandler<ImportMarker> importRequestHandler,
        IAPIRequestHandler<WTMarker> wtRequestHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _importRequestHandler = importRequestHandler ?? throw new ArgumentNullException(nameof(importRequestHandler));
        _wtRequestHandler = wtRequestHandler ?? throw new ArgumentNullException(nameof(wtRequestHandler));
    }

    private async Task<string> ReadRequestBodyAsync()
    {
        using var reader = new StreamReader(Request.Body);
        return await reader.ReadToEndAsync();
    }

    /// <summary xml:lang = "ru">
    /// Принимает запрос на обработку оповещения от импорта.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    [HttpPost("import")]
    public async Task HandleImportNotificationAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new notification from 'import'");

        await _importRequestHandler.HandleAsync(body);
    }

    /// <summary xml:lang = "ru">
    /// Принимает запрос на обработку оповещения от WT.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    [HttpPost("wt")]
    public async Task HandleHornsAndHoovesProductsAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new notification from 'wt'");

        await _wtRequestHandler.HandleAsync(body);
    }
}
