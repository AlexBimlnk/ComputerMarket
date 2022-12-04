using General.Logic.Commands;
using General.Logic.Executables;
using General.Storage;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда возврата средств по запросу.
/// </summary>
public sealed class RefundTransactionRequestCommand : CommandBase, ICommand
{
    private readonly IKeyableCache<TransactionRequest, InternalID> _requestCache;
    private readonly RefundTransactionRequestCommandParameters _parameters;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="RefundTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры команды.
    /// </param>
    /// <param name="requestCache" xml:lang = "ru">
    /// Кэш запросов.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public RefundTransactionRequestCommand(
        RefundTransactionRequestCommandParameters parameters,
        IKeyableCache<TransactionRequest, InternalID> requestCache)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
    }

    /// <inheritdoc/>
    public override ExecutableID Id => _parameters.Id;

    protected override Task ExecuteCoreAsync() =>
        // some logic
        Task.CompletedTask;
}
