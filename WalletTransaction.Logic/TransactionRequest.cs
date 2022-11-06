using General.Models;

namespace WalletTransaction.Logic;

public sealed class TransactionRequest : IKeyable<InternalID>
{
    public TransactionRequest(
        InternalID id,
        IReadOnlyCollection<Transaction> transactions)
    {
        Id = id;
        Transactions = transactions;
    }

    public InternalID Id { get; }
    public InternalID Key => Id;
    public IReadOnlyCollection<Transaction> Transactions { get; }
}
