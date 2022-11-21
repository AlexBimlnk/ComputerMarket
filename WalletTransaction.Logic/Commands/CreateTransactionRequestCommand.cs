using General.Logic.Commands;
using General.Storage;
using General.Transport;

using WalletTransaction.Logic.Transport.Configurations;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда создания запроса на проведение транзакций.
/// </summary>
public sealed class CreateTransactionRequestCommand : CommandBase, ICommand
{
    private readonly ISender<TransactionSenderConfiguration, ITransactionsRequest> _sender;
    private readonly IKeyableCache<TransactionRequest, InternalID> _requestCache;
    private readonly CreateTransactionRequestCommandParameters _parameters;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="CreateTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры команды.
    /// </param>
    /// <param name="sender" xml:lang = "ru">
    /// Отправитель запросов.
    /// </param>
    /// <param name="requestCache" xml:lang = "ru">
    /// Кэш запросов.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public CreateTransactionRequestCommand(
        CreateTransactionRequestCommandParameters parameters,
        ISender<TransactionSenderConfiguration, ITransactionsRequest> sender,
        IKeyableCache<TransactionRequest, InternalID> requestCache)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
    }

    /// <inheritdoc/>
    public override CommandID Id => _parameters.Id;

    protected override async Task ExecuteCoreAsync()
    {
        _requestCache.Add(_parameters.TransactionRequest);

        var hasOtherProvider = _parameters.TransactionRequest.Transactions
            .Any(x => x.To != ToMarketProxyTransactionRequest.MarketAccount);

        if (hasOtherProvider)
        {
            await _sender.SendAsync(new ToMarketProxyTransactionRequest(_parameters.TransactionRequest));
        }
        else
        {
            await _sender.SendAsync(_parameters.TransactionRequest);
        }
    }
}
