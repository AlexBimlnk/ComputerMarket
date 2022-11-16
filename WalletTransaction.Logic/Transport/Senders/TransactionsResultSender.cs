using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using General.Transport;

using WalletTransaction.Logic.Transport.Configurations;

namespace WalletTransaction.Logic.Transport.Senders;


public sealed class TransactionsResultSender : ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>
{
    public Task SendAsync(ITransactionsRequest entity, CancellationToken token = default) => throw new NotImplementedException();
}
