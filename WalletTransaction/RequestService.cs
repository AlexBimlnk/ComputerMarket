using WalletTransaction.Logic;

namespace WalletTransaction;

/// <summary xml:lang = "ru">
/// Фоновый сервис, занимающийся обработкой полученных запросов на проведение транзакций.
/// </summary>
public sealed class RequestService : BackgroundService
{
    private readonly ILogger<RequestService> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ITransactionsRequestProcessor _processor;

    /// <summary xml:lang = "ru">
    /// Создает новый объект типа <see cref="RequestService"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="applicationLifetime" xml:lang = "ru">
    /// Жизненный цикл приложения.
    /// </param>
    /// <param name="processor" xml:lang = "ru">
    /// Обработчик транзакций.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public RequestService(
        ILogger<RequestService> logger,
        IHostApplicationLifetime applicationLifetime,
        ITransactionsRequestProcessor processor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _processor.ProcessAsync(stoppingToken);
        _applicationLifetime.StopApplication();
    }
}
