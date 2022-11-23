namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Описывает выполнителя запросов транзакций.
/// </summary>
public interface ITransactionRequestExecuter
{
    /// <summary xml:lang = "ru">
    /// Выполняет запрос на проведение транзакций.
    /// </summary>
    /// <param name="transactionRequest" xml:lang = "ru">
    /// Запрос на обработку транзакций.
    /// </param>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены.
    /// </param>
    /// <returns xml:lang = "ru"> <see cref="Task"/>. </returns>
    public Task ExecuteAsync(ITransactionsRequest transactionRequest, CancellationToken token);

    /// <summary xml:lang = "ru">
    /// Возврат денежных средств по запросу.
    /// </summary>
    /// <param name="transactionRequest" xml:lang = "ru">
    /// Запрос на обработку транзакций.
    /// </param>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены.
    /// </param>
    /// <returns xml:lang = "ru"> <see cref="Task"/>. </returns>
    public Task RefundAsync(ITransactionsRequest transactionRequest, CancellationToken token);
}
