using General.Storage;

namespace WalletTransaction.Logic;

public interface ITransactionRequestCache : IKeyableCache<TransactionRequest, InternalID>
{
    public void CancelRequest(InternalID requestId);
}
