using System.Threading.Channels;

namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Очередь транзакций, которые нужно обработать.
/// </summary>
public sealed class ProcessingTransactionsChannel : ITransactionsChannel
{
    private readonly Channel<(ITransactionsRequest, CancellationToken)> _channel = Channel.CreateUnbounded<(ITransactionsRequest, CancellationToken)>();

    /// <summary xml:lang = "ru">
    /// Читатель очереди.
    /// </summary>
    public ChannelReader<(ITransactionsRequest, CancellationToken)> ChannelReader => _channel.Reader;

    /// <summary xml:lang = "ru">
    /// Писатель очереди.
    /// </summary>
    public ChannelWriter<(ITransactionsRequest, CancellationToken)> ChannelWriter => _channel.Writer;
}
