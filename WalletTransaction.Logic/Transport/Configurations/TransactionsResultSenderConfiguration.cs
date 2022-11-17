using General.Transport;

namespace WalletTransaction.Logic.Transport.Configurations;

/// <summary xml:lang = "ru">
/// Конфигурация отправителя результатов обработки транзакций.
/// </summary>
public sealed class TransactionsResultSenderConfiguration : ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public string Destination { get; set; } = null!;
}
