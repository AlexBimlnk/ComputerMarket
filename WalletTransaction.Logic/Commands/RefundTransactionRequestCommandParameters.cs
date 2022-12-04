using General.Logic.Commands;
using General.Logic.Executables;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Параметры команды для возврата средств по запросу.
/// </summary>
public sealed class RefundTransactionRequestCommandParameters : CommandParametersBase
{
    /// <summary xml:lang = "ru"> 
    /// Создает новый экземпляр типа <see cref="RefundTransactionRequestCommandParameters"/>.
    /// </summary>
    /// <param name="id"> 
    /// Идентификатор команды. 
    /// </param>
    /// <param name="requestId"> 
    /// Идентификатор запроса который необходимо отменить. 
    /// </param>
    public RefundTransactionRequestCommandParameters(
        ExecutableID id,
        InternalID requestId) : base(id)
    {
        TransactionRequestId = requestId;
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса на проведение транзакций.
    /// </summary>
    public InternalID TransactionRequestId { get; }
}
