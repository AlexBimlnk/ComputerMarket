using Market.Logic.Commands;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Models.Commands.WT;

/// <summary xml:lang = "ru">
/// Транспортная модель команды для полного рассчета запроса на проведение транзакции.
/// </summary>
public sealed class FinishTransactionRequestCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="FinishTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public FinishTransactionRequestCommand(string id, CommandType type) : base(id, type) { }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса на проведение транзакции.
    /// </summary>
    [JsonProperty("request_id", Required = Required.Always)]
    public long TransactionRequestID { get; set; }

    public static FinishTransactionRequestCommand ToModel(Logic.Commands.WT.FinishTransactionRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new FinishTransactionRequestCommand(command.Id.Value, command.Type)
        {
            TransactionRequestID = command.TransactionRequestID.Value
        };
    }
}