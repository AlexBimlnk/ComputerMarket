using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletTransaction.Logic;
public interface ITransactionsRequest
{
    public InternalID Id { get; }
    public IReadOnlyCollection<Transaction> Transactions { get; }
}
