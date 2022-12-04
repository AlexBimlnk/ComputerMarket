using Newtonsoft.Json;

namespace WalletTransaction.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Транспортная модель команды возврата средств по запросу.
/// </summary>
public sealed class RefundRequestCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="RefundRequestCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public RefundRequestCommand(CommandType type, string id) : base(type, id) { }

    /// <summary xml:lang = "ru">
    /// Идентификатор запроса.
    /// </summary>
    [JsonProperty("request_id", Required = Required.Always)]
    public long RequestID { get; set; }
}
