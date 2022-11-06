namespace WalletTransaction.Logic;
public class MarketProxyTransactionRequest : ITransactionsRequest
{
    private static readonly BankAccount s_marketAccount = new("01234012340123401234");

    private readonly ITransactionsRequest _wrappedRequest;

    public MarketProxyTransactionRequest(ITransactionsRequest transactionRequest)
    {
        _wrappedRequest = transactionRequest ?? throw new ArgumentNullException(nameof(transactionRequest));
    }

    public InternalID Id => _wrappedRequest.Id;

    public IReadOnlyCollection<Transaction> Transactions => GetTransactions();

    private IReadOnlyCollection<Transaction> GetTransactions()
    {
        var fromAccount = _wrappedRequest.Transactions
            .Select(x => x.From)
            .Distinct()
            .Single();

        var transferedBalace = _wrappedRequest.Transactions
            .Select(x => x.TransferBalance)
            .Sum();

        var transaction = new Transaction(fromAccount, s_marketAccount, transferedBalace);

        return new List<Transaction>(1) { transaction };
    }
}
