using Market.Logic.Commands;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Models.Commands.Import;

/// <summary xml:lang = "ru">
/// Транспортная модель команды для отмены запроса на проведение транзакции.
/// </summary>
public sealed class CancelTransactionRequestCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CancelTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public CancelTransactionRequestCommand(string id, CommandType type) : base(id, type) { }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса на проведение транзакции.
    /// </summary>
    [JsonProperty("request_id", Required = Required.Always)]
    public long TransactionRequestID { get; set; }

    public static CancelTransactionRequestCommand ToModel(Logic.Commands.WT.CancelTransactionRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new CancelTransactionRequestCommand(command.Id.Value, command.Type)
        {
            TransactionRequestID = command.TransactionRequestID.Value
        };
    }
}