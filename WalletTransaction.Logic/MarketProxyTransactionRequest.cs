namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Посредник, перенаправляющий транзации на счёт магазина.
/// </summary>
public class MarketProxyTransactionRequest : ITransactionsRequest
{
    private static readonly BankAccount s_marketAccount = new("01234012340123401234");

    private readonly ITransactionsRequest _wrappedRequest;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="MarketProxyTransactionRequest"/>.
    /// </summary>
    /// <param name="transactionRequest" xml:lang = "ru">
    /// Оборачиваемый запрос на проведение транзакций.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="transactionRequest"/> оказался <see langword="null"/>.
    /// </exception>
    public MarketProxyTransactionRequest(ITransactionsRequest transactionRequest)
    {
        _wrappedRequest = transactionRequest ?? throw new ArgumentNullException(nameof(transactionRequest));
    }

    public static BankAccount MarketAccount => s_marketAccount;

    /// <inheritdoc/>
    public InternalID Id => _wrappedRequest.Id;

    /// <inheritdoc/>
    public IReadOnlyCollection<Transaction> Transactions => GetTransactions();

    /// <inheritdoc/>
    public bool IsCancelled => _wrappedRequest.IsCancelled;

    /// <inheritdoc/>
    public TransactionRequestState CurrentState
    {
        get => _wrappedRequest.CurrentState;
        set
        {
            if (value is TransactionRequestState.Finished)
                _wrappedRequest.CurrentState = TransactionRequestState.Held;
            _wrappedRequest.CurrentState = value;
        }
    }

    /// <inheritdoc/>
    public TransactionRequestState OldState => _wrappedRequest.OldState;

    private IReadOnlyCollection<Transaction> GetTransactions()
    {
        var fromAccount = _wrappedRequest.Transactions
            .Select(x => x.From)
            .Distinct()
            .Single();

        var transferedBalace = _wrappedRequest.Transactions
            .Select(x => x.TransferBalance)
            .Sum();

        var transaction = new Transaction(fromAccount, MarketAccount, transferedBalace);

        return new List<Transaction>(1) { transaction };
    }
}
