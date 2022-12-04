using Market.Logic.Commands;
using Market.Logic.Models.WT;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Models.Commands.WT;

/// <summary xml:lang = "ru">
/// Транспортная модель команды для создания запроса на проведение транзакции.
/// </summary>
public sealed class CreateTransactionRequestCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CreateTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public CreateTransactionRequestCommand(string id, CommandType type) : base(id, type) { }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса на проведение транзакции.
    /// </summary>
    [JsonProperty("request", Required = Required.Always)]
    public Request TransactionRequest { get; set; } = default!;

    public static CreateTransactionRequestCommand ToModel(Logic.Commands.WT.CreateTransactionRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new CreateTransactionRequestCommand(command.Id.Value, command.Type)
        {
            TransactionRequest = new Request
            {
                TransactionRequestID = command.TransactionRequestID.Value,
                Transactions = command.Transactions
                    .Select(x => TransportTransaction.ToModel(x))
                    .ToList()
            }
        };
    }

    public sealed class Request
    {
        /// <summary xml:lang = "ru">
        /// Идентификатор запроса на проведение транзакции.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public long TransactionRequestID { get; set; }

        /// <summary xml:lang = "ru">
        /// Коллекция транзакций, которые нужно выполнить в рамках запроса.
        /// </summary>
        [JsonProperty("transactions", Required = Required.Always)]
        public IReadOnlyCollection<TransportTransaction> Transactions { get; set; } = default!;
    }

    public sealed class TransportTransaction
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
        /// Конверитрует транзакцию из доменной модели в транспортную.
        /// </summary>
        /// <param name="transaction">
        /// Доменная модель транзакции.
        /// </param>
        /// <returns>
        /// Транспортная модель модель <see cref="TransportTransaction"/>.
        /// </returns>
        public static TransportTransaction ToModel(Transaction transaction) =>
            new TransportTransaction
            {
                From = transaction.From,
                To = transaction.To,
                TransferBalance = transaction.TransferBalance,
                HeldBalance = transaction.HeldBalance,
            };
    }
}