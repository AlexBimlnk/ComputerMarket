using General.Logic.Commands;
using General.Logic.Executables;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Параметры команды для создания нового запроса на проведение транзакций.
/// </summary>
public sealed class CreateTransactionRequestCommandParameters : CommandParametersBase
{
    /// <summary xml:lang = "ru"> 
    /// Создает новый экземпляр типа <see cref="CreateTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="id"> 
    /// Идентификатор команды. 
    /// </param>
    /// <param name="transactionRequest">
    /// Запрос на проведение транзакций.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Когда любой из параметров оказался <see langword="null"/>.
    /// </exception>
    public CreateTransactionRequestCommandParameters(
        ExecutableID id,
        TransactionRequest transactionRequest) : base(id)
    {
        TransactionRequest = transactionRequest ?? throw new ArgumentNullException(nameof(transactionRequest));
    }

    /// <summary xml:lang = "ru">
    /// Запрос на проведение транзакций.
    /// </summary>
    public TransactionRequest TransactionRequest { get; }
}
