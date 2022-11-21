using General.Transport;

namespace WalletTransaction.Logic.Transport.Configurations;

/// <summary xml:lang = "ru">
/// Конфигурация отправителя транзакций.
/// </summary>
public sealed class TransactionSenderConfiguration : ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public string Destination { get; set; } = default!;
}
