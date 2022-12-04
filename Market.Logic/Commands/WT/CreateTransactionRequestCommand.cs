using General.Logic.Executables;

using Market.Logic.Commands.Import;
using Market.Logic.Models.WT;

namespace Market.Logic.Commands.WT;

/// <summary xml:lang = "ru">
/// Комманда на создание запроса на проведение транзакции.
/// </summary>
public sealed class CreateTransactionRequestCommand : WTCommand
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CreateTransactionRequestCommand"/>.
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
    public CreateTransactionRequestCommand(
        ExecutableID id,
        ID requestID,
        IReadOnlyCollection<Transaction> transactions)
        : base(id, CommandType.CreateTransactionRequest)
    {
        TransactionRequestID = requestID;
        Transactions = transactions ?? throw new ArgumentNullException(nameof(transactions));
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    public ID TransactionRequestID { get; }

    /// <summary xml:lang = "ru">
    /// Транзакции в рамках запроса.
    /// </summary>
    public IReadOnlyCollection<Transaction> Transactions { get; }
}
