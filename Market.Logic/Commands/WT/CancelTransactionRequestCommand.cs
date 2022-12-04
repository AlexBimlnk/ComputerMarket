using General.Logic.Executables;

using Market.Logic.Commands.Import;

namespace Market.Logic.Commands.WT;

/// <summary xml:lang = "ru">
/// Комманда на отмену запроса на проведение транзакции.
/// </summary>
public sealed class CancelTransactionRequestCommand : WTCommand
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CancelTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <param name="requestID" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="provider"/> оказался <see langword="null"/>.
    /// </exception>
    public CancelTransactionRequestCommand(
        ExecutableID id,
        ID requestID)
        : base(id, CommandType.CancelTransactionRequest)
    {
        TransactionRequestID = requestID;
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    public ID TransactionRequestID { get; }
}
