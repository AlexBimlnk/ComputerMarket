namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Описывает обработчика запросов на выполнение транзакций.
/// </summary>
public interface ITransactionsRequestProcessor
{
    /// <summary xml:lang = "ru">
    /// Обработка запроса.
    /// </summary>
    /// <param name="transactionRequest" xml:lang = "ru">
    /// Запрос на обработку транзакций.
    /// </param>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены.
    /// </param>
    /// <returns xml:lang = "ru"> <see cref="Task"/>. </returns>
    public Task ProcessAsync(ITransactionsRequest transactionRequest, CancellationToken token);

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
