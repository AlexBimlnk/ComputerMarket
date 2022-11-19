namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Выполнитель запросов на проведение транзакций.
/// </summary>
public sealed class TransactionRequestExecuter : ITransactionRequestExecuter
{
    private static async Task ProcessTransactionAsync(Transaction transaction, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        await Task.Delay(TimeSpan.FromMinutes(2), token);

        // Симулирование ошибки транзакции (транзакция не выполнена)
        if (Random.Shared.NextInt64() % 5 == 0)
            return;

        transaction.Complete();
    }

    /// <inheritdoc/>
    public async Task ExecuteAsync(ITransactionsRequest transactionRequest, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(transactionRequest);

        await Task.WhenAll(transactionRequest.Transactions
            .Select(x => ProcessTransactionAsync(x, token)));

        var isSuccessful = transactionRequest.Transactions
            .All(x => x.IsCompleted);

        if (isSuccessful)
            transactionRequest.CurrentState = TransactionRequestState.Finished;
        else
            transactionRequest.CurrentState = TransactionRequestState.Aborted;
    }

    /// <inheritdoc/>
    public async Task RefundAsync(ITransactionsRequest transactionRequest, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(transactionRequest);

        // some other logic

        await Task.WhenAll(transactionRequest.Transactions
            .Select(x => ProcessTransactionAsync(x, token)));

        transactionRequest.CurrentState = TransactionRequestState.Refunded;
    }
}
