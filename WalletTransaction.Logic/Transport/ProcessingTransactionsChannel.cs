using System.Threading.Channels;

using General.Transport;

namespace WalletTransaction.Logic.Transport;

/// <summary xml:lang = "ru">
/// Очередь транзакций, которые нужно обработать.
/// </summary>
public sealed class ProcessingTransactionsChannel : 
    IReceiver<ITransactionsRequest>, 
    ISender<TransactionSenderConfiguration, ITransactionsRequest>
{
    private readonly Channel<ITransactionsRequest> _channel = Channel.CreateUnbounded<ITransactionsRequest>();

    /// <inheritdoc/>
    public async Task<ITransactionsRequest> ReceiveAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return await _channel.Reader.ReadAsync(token);
    }

    /// <inheritdoc/>
    public async Task SendAsync(ITransactionsRequest entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _channel.Writer.WriteAsync(entity, token);
    }
}
