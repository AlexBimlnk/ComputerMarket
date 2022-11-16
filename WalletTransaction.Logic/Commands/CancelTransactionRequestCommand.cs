using General.Logic.Commands;
using General.Storage;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда отмены запроса на проведение транзакций.
/// </summary>
public sealed class CancelTransactionRequestCommand : CommandBase, ICommand
{
    private readonly IKeyableCache<TransactionRequest, InternalID> _requestCache;
    private readonly CancelTransactionRequestCommandParameters _parameters;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="CancelTransactionRequestCommand"/>.
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
    public CancelTransactionRequestCommand(
        CancelTransactionRequestCommandParameters parameters,
        IKeyableCache<TransactionRequest, InternalID> requestCache)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
    }

    /// <inheritdoc/>
    public override CommandID Id => _parameters.Id;

    protected override Task ExecuteCoreAsync()
    {
        var request = _requestCache.GetByKey(_parameters.TransactionRequestId);

        if (request is null)
            throw new InvalidOperationException(
                $"Transaction request with id {_parameters.TransactionRequestId} is not exists.");

        request.IsCancelled = true;

        _requestCache.Delete(request);

        return Task.CompletedTask;
    }
}
