using General.Transport;

namespace WalletTransaction.Logic.Transport;

/// <summary xml:lang = "ru">
/// Конфигурация отправителя транзакций.
/// </summary>
public sealed class TransactionSenderConfiguration : ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public string Destination => throw new NotImplementedException();
}
