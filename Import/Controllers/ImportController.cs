using Import.Logic.Abstractions;
using Import.Logic.Transport.Models;

using Microsoft.AspNetCore.Mvc;

namespace Import.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер импорта.
/// </summary>
[ApiController]
[Route("import/[controller]")]
public class ImportController : ControllerBase
{
    private readonly ILogger<ImportController> _logger;
    private readonly IAPIExternalProductHandler<ExternalProduct> _ivanovHandler;
    private readonly IAPIExternalProductHandler<HornsAndHoovesProduct> _hornsAndHoovesHandler;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="ImportController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="ivanovHandler" xml:lang = "ru">
    /// Обработчик продуктов от <see cref="Logic.Models.Provider.Ivanov"/>.
    /// </param>
    /// <param name="hornsAndHoovesHandler" xml:lang = "ru">
    /// Обработчик продуктов от <see cref="Logic.Models.Provider.HornsAndHooves"/>.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public ImportController(
        ILogger<ImportController> logger,
        IAPIExternalProductHandler<ExternalProduct> ivanovHandler,
        IAPIExternalProductHandler<HornsAndHoovesProduct> hornsAndHoovesHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _ivanovHandler = ivanovHandler ?? throw new ArgumentNullException(nameof(ivanovHandler));
        _hornsAndHoovesHandler = hornsAndHoovesHandler ?? throw new ArgumentNullException(nameof(hornsAndHoovesHandler));
    }

    private async Task<string> ReadRequestBodyAsync()
    {
        using var reader = new StreamReader(Request.Body);
        return await reader.ReadToEndAsync();
    }

    /// <summary xml:lang = "ru">
    /// Принимает запрос на обработку внешних продуктов 
    /// от <see cref="Logic.Models.Provider.Ivanov"/>.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    [HttpGet("product/ivanov")]
    public async Task HandleIvanovProductsAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request from 'ivanov'");

        await _ivanovHandler.HandleAsync(body);
    }

    /// <summary xml:lang = "ru">
    /// Принимает запрос на обработку внешних продуктов 
    /// от <see cref="Logic.Models.Provider.HornsAndHooves"/>.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    [HttpGet("product/hah")]
    public async Task HandleHornsAndHoovesProductsAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request from 'horns and hooves'");

        await _hornsAndHoovesHandler.HandleAsync(body);
    }
}
