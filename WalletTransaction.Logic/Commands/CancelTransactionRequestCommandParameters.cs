using General.Logic.Commands;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Параметры команды для отмены запроса на проведение транзакций.
/// </summary>
public sealed class CancelTransactionRequestCommandParameters : CommandParametersBase
{
    /// <summary xml:lang = "ru"> 
    /// Создает новый экземпляр типа <see cref="CancelTransactionRequestCommandParameters"/>.
    /// </summary>
    /// <param name="id"> 
    /// Идентификатор запроса который необходимо отменить. 
    /// </param>
    public CancelTransactionRequestCommandParameters(
        CommandID id,
        InternalID requestId) : base(id)
    {
        TransactionRequestId = requestId;
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса на проведение транзакций.
    /// </summary>
    public InternalID TransactionRequestId { get; }
}
