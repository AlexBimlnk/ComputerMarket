namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Описывает состояния, в которых может 
/// находится запрос на проведение транзакций.
/// </summary>
public enum TransactionRequestState
{
    /// <summary xml:lang = "ru">
    /// Ожидает обработки.
    /// </summary>
    WaitHandle,

    /// <summary xml:lang = "ru">
    /// Удерживается магазином.
    /// </summary>
    Held,

    /// <summary xml:lang = "ru">
    /// Оплата дошла до всех поставщиков.
    /// </summary>
    Finished,

    /// <summary xml:lang = "ru">
    /// Произведен возврат средств.
    /// </summary>
    Refunded,

    /// <summary xml:lang = "ru"> 
    /// Критическое состояние ошибки 
    /// во время исполнения транзакции.
    /// </summary>
    Aborted
}
