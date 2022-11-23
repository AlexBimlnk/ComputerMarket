namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Посредник, перенаправляющий транзации со счёт магазина.
/// </summary>
public class FromMarketProxyTransactionRequest : ITransactionsRequest
{
    private static readonly BankAccount s_marketAccount = new("01234012340123401234");

    private readonly ITransactionsRequest _wrappedRequest;
    private IReadOnlyCollection<Transaction> _proxyTransactions = null!;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="ToMarketProxyTransactionRequest"/>.
    /// </summary>
    /// <param name="transactionRequest" xml:lang = "ru">
    /// Оборачиваемый запрос на проведение транзакций.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="transactionRequest"/> оказался <see langword="null"/>.
    /// </exception>
    public FromMarketProxyTransactionRequest(ITransactionsRequest transactionRequest)
    {
        _wrappedRequest = transactionRequest ?? throw new ArgumentNullException(nameof(transactionRequest));
    }

    public static BankAccount MarketAccount => s_marketAccount;

    /// <inheritdoc/>
    public InternalID Id => _wrappedRequest.Id;

    /// <inheritdoc/>
    public IReadOnlyCollection<Transaction> Transactions
    {
        get
        {
            if (_proxyTransactions is null)
                _proxyTransactions = GetTransactions();
            return _proxyTransactions;
        }
    }

    /// <inheritdoc/>
    public bool IsCancelled => _wrappedRequest.IsCancelled;

    /// <inheritdoc/>
    public TransactionRequestState CurrentState
    {
        get => _wrappedRequest.CurrentState;
        set => _wrappedRequest.CurrentState = value;
    }

    /// <inheritdoc/>
    public TransactionRequestState OldState => _wrappedRequest.OldState;

    private IReadOnlyCollection<Transaction> GetTransactions() =>
        _wrappedRequest.Transactions
            .Select(x => new Transaction(
                MarketAccount,
                x.To,
                x.TransferBalance,
                x.HeldBalance))
            .ToList();
}
