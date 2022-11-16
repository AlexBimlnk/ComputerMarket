using System.Threading.Channels;

namespace WalletTransaction.Logic;

/// <summary>
/// Описывает очередь транзакций.
/// </summary>
public interface ITransactionsChannel
{
    /// <summary xml:lang = "ru">
    /// Читатель очереди.
    /// </summary>
    ChannelReader<(ITransactionsRequest, CancellationToken)> ChannelReader { get; }

    /// <summary xml:lang = "ru">
    /// Писатель очереди.
    /// </summary>
    ChannelWriter<(ITransactionsRequest, CancellationToken)> ChannelWriter { get; }
}