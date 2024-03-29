using General.Logic;

using Import.Logic.Abstractions;
using Import.Logic.Transport.Models;

using Microsoft.AspNetCore.Mvc;

namespace Import.Controllers;

/// <summary xml:lang = "ru">
/// ���������� �������.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    private readonly ILogger<ImportController> _logger;
    private readonly IAPIRequestHandler<ExternalProduct> _ivanovHandler;
    private readonly IAPIRequestHandler<HornsAndHoovesProduct> _hornsAndHoovesHandler;

    /// <summary xml:lang = "ru">
    /// ������ ����� ��������� ���� <see cref="ImportController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// ������.
    /// </param>
    /// <param name="ivanovHandler" xml:lang = "ru">
    /// ���������� ��������� �� <see cref="Logic.Models.Provider.Ivanov"/>.
    /// </param>
    /// <param name="hornsAndHoovesHandler" xml:lang = "ru">
    /// ���������� ��������� �� <see cref="Logic.Models.Provider.HornsAndHooves"/>.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// ����� ����� �� ������� ���������� �������� <see langword="null"/>.
    /// </exception>
    public ImportController(
        ILogger<ImportController> logger,
        IAPIRequestHandler<ExternalProduct> ivanovHandler,
        IAPIRequestHandler<HornsAndHoovesProduct> hornsAndHoovesHandler)
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
    /// ��������� ������ �� ��������� ������� ��������� 
    /// �� <see cref="Logic.Models.Provider.Ivanov"/>.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    [HttpPost("product/ivanov")]
    public async Task HandleIvanovProductsAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request from 'ivanov'");

        await _ivanovHandler.HandleAsync(body);
    }

    /// <summary xml:lang = "ru">
    /// ��������� ������ �� ��������� ������� ��������� 
    /// �� <see cref="Logic.Models.Provider.HornsAndHooves"/>.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    [HttpPost("product/hah")]
    public async Task HandleHornsAndHoovesProductsAsync()
    {
        var body = await ReadRequestBodyAsync();

        _logger.LogDebug("Given new request from 'horns and hooves'");

        await _hornsAndHoovesHandler.HandleAsync(body);
    }
}
