using Newtonsoft.Json;

namespace WalletTransaction.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Транспортная модель команды отмены запроса.
/// </summary>
public sealed class CancelRequestCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CancelRequestCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public CancelRequestCommand(CommandType type, string id) : base(type, id) { }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    [JsonProperty("request_id", Required = Required.Always)]
    public long RequestID { get; set; }
}
