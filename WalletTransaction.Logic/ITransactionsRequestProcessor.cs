namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Описывает обработчика запросов на выполнение транзакций.
/// </summary>
public interface ITransactionsRequestProcessor
{
    /// <summary xml:lang = "ru">
    /// Начинает обработку запросов.
    /// </summary>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены.
    /// </param>
    /// <returns xml:lang = "ru"> <see cref="Task"/>. </returns>
    public Task ProcessAsync(CancellationToken token = default);
}
