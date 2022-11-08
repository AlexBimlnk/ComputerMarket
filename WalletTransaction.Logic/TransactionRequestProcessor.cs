namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Обработчик запросов на проведение транзакций.
/// </summary>
public sealed class TransactionRequestProcessor : ITransactionsRequestProcessor
{
    private static async Task ProcessTransactionAsync(Transaction transaction, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        await Task.Delay(TimeSpan.FromMinutes(2), token);

        transaction.Complete();
    }

    /// <inheritdoc/>
    public async Task ProcessAsync(ITransactionsRequest transactionRequest, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(transactionRequest);

        await Task.WhenAll(transactionRequest.Transactions
            .Select(x => ProcessTransactionAsync(x, token)));
    }

    /// <inheritdoc/>
    public async Task RefundAsync(ITransactionsRequest transactionRequest, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(transactionRequest);

        // some other logic

        await Task.WhenAll(transactionRequest.Transactions
            .Select(x => ProcessTransactionAsync(x, token)));
    }
}
