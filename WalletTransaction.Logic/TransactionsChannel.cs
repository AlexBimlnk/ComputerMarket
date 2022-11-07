using System.Threading.Channels;

namespace WalletTransaction.Logic;
public sealed class TransactionsChannel
{
    private readonly Channel<(ITransactionsRequest, CancellationToken)> _channel = Channel.CreateUnbounded<(ITransactionsRequest, CancellationToken)>();

    public ChannelReader<(ITransactionsRequest, CancellationToken)> ChannelReader => _channel.Reader;

    public ChannelWriter<(ITransactionsRequest, CancellationToken)> ChannelWriter => _channel.Writer;
}
