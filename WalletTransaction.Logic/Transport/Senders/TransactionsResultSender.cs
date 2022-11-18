using General.Transport;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using WalletTransaction.Logic.Transport.Configurations;

namespace WalletTransaction.Logic.Transport.Senders;

/// <summary xml:lang = "ru">
/// Отправитель результатов обработки запросов на проведение транзакций.
/// </summary>
public sealed class TransactionsResultSender :
    ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>
{
    private readonly ILogger<TransactionsResultSender> _logger;
    private readonly TransactionsResultSenderConfiguration _configuration;
    private readonly ISerializer<ITransactionsRequest, string> _serializer;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпля типа <see cref="TransactionsResultSender"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="options" xml:lang = "ru">
    /// Опции с конфигурацией отправителя.
    /// </param>
    /// <param name="serializer" xml:lang = "ru">
    /// Сериализатор результатов обработки запроса на проведение транзакций.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из параметров оказался <see langword="null"/>.
    /// </exception>
    public TransactionsResultSender(
        ILogger<TransactionsResultSender> logger,
        IOptions<TransactionsResultSenderConfiguration> options,
        ISerializer<ITransactionsRequest, string> serializer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task SendAsync(ITransactionsRequest entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Sending result request with id {RequestId}", entity.Id);

        var request = _serializer.Serialize(entity);
        using var content = new StringContent(request);

        using var client = new HttpClient();

        HttpResponseMessage response = null!;

        try
        {
            response = await client.PostAsync(_configuration.Destination, content, token)
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                _logger.LogInformation("The request result have been successfully sended");
            else
                _logger.LogWarning(
                    "The result request have not been sended. Response status code: {Status code}",
                    response.StatusCode);
        }
        catch (Exception ex)
            when (ex is InvalidOperationException or HttpRequestException)
        {
            _logger.LogWarning(
                "The result request have not been sended. More info: {Info}",
                ex.Message);
        }
        finally
        {
            response?.Dispose();
        }
    }
}
