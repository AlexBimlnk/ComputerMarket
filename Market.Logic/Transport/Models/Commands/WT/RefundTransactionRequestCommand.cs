using Market.Logic.Commands;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Models.Commands.Import;

/// <summary xml:lang = "ru">
/// Транспортная модель команды для возврата средств по транзакции.
/// </summary>
public sealed class RefundTransactionRequestCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="RefundTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public RefundTransactionRequestCommand(string id, CommandType type) : base(id, type) { }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса на проведение транзакции.
    /// </summary>
    [JsonProperty("request_id", Required = Required.Always)]
    public long TransactionRequestID { get; set; }

    public static RefundTransactionRequestCommand ToModel(Logic.Commands.WT.RefundTransactionRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new RefundTransactionRequestCommand(command.Id.Value, command.Type)
        {
            TransactionRequestID = command.TransactionRequestID.Value
        };
    }
}