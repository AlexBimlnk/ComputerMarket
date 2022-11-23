using Newtonsoft.Json;

namespace WalletTransaction.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель транзакции.
/// </summary>
public sealed class Transaction
{
    /// <summary xml:lang = "ru">
    /// Счет отправителя.
    /// </summary>
    [JsonProperty("from", Required = Required.Always)]
    public string From { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Счет получателя.
    /// </summary>
    [JsonProperty("to", Required = Required.Always)]
    public string To { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Передаваемый баланс.
    /// </summary>
    [JsonProperty("transfer_balance", Required = Required.Always)]
    public decimal TransferBalance { get; set; }

    /// <summary xml:lang = "ru">
    /// Удерживаемый баланс.
    /// </summary>
    [JsonProperty("held_balance", Required = Required.Always)]
    public decimal HeldBalance { get; set; }

    /// <summary xml:lang = "ru">
    /// Конверитрует транзакцию из транспортной модели в доменную.
    /// </summary>
    /// <param name="transaction">
    /// Транспортная модель транзакции.
    /// </param>
    /// <returns>
    /// Доменную модель <see cref="Logic.Transaction"/>.
    /// </returns>
    public static Logic.Transaction FromModel(Transaction transaction) =>
        new Logic.Transaction(
            new(transaction.From),
            new(transaction.To),
            transaction.TransferBalance,
            transaction.HeldBalance);
}
