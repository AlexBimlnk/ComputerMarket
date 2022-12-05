using General.Transport;

using Microsoft.Extensions.Logging;

using WalletTransaction.Logic.Transport.Configurations;

namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Обработчик запросов на проведение транзакций.
/// </summary>
public sealed class TransactionRequestProcessor : ITransactionsRequestProcessor
{
    private readonly ILogger<TransactionRequestProcessor> _logger;
    private readonly IReceiver<ITransactionsRequest> _receiver;
    private readonly ISender<TransactionsResultSenderConfiguration, ITransactionsRequest> _resultSender;
    private readonly ITransactionRequestExecuter _transactionRequestExecuter;

    /// <summary  xml:lang = "ru">
    /// <see cref="TransactionRequestProcessor"/>
    /// </summary>
    /// <param name="logger"  xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="receiver"  xml:lang = "ru">
    /// Получатель запросов на проведение транзакций.
    /// </param>
    /// <param name="resultSender"  xml:lang = "ru">
    /// Отправитель результатов о проведении транзакции.
    /// </param>
    /// <param name="transactionRequestExecuter">
    /// Исполнитель запросов на проведение транзакций.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из параметров оказался <see langword="null"/>.
    /// </exception>
    public TransactionRequestProcessor(
        ILogger<TransactionRequestProcessor> logger, 
        IReceiver<ITransactionsRequest> receiver, 
        ISender<TransactionsResultSenderConfiguration, ITransactionsRequest> resultSender, 
        ITransactionRequestExecuter transactionRequestExecuter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _resultSender = resultSender ?? throw new ArgumentNullException(nameof(resultSender));
        _transactionRequestExecuter = transactionRequestExecuter ?? throw new ArgumentNullException(nameof(transactionRequestExecuter));
    }

    /// <inheritdoc/>
    public async Task ProcessAsync(CancellationToken token = default)
    {
        _logger.LogInformation("Start receive and process requests..");

        while (true)
        {
            token.ThrowIfCancellationRequested();

            var request = await _receiver.ReceiveAsync(token);

            _logger.LogDebug("Received new request");

            if (request.OldState is TransactionRequestState.Finished)
            {
                _logger.LogWarning("Request already finished! Skip");
                return;
            }

            if (request.OldState is TransactionRequestState.Aborted)
            {
                _logger.LogWarning("Request has aborted status");
                return;
            }

            if (!request.IsCancelled)
                await _transactionRequestExecuter.ExecuteAsync(request, token);

            _logger.LogDebug(
                "Processing comlete with request status: {Status}",
                nameof(request.CurrentState));

            await _resultSender.SendAsync(request, token);

            _logger.LogDebug("Sending result...");

            _logger.LogDebug("Request has been processed");
        }
    }
}
