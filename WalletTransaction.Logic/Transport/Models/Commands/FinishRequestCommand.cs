using Newtonsoft.Json;

namespace WalletTransaction.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Транспортная модель команды полного рассчета запроса.
/// </summary>
public sealed class FinishRequestCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="FinishRequestCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public FinishRequestCommand(CommandType type, string id) : base(type, id) { }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    [JsonProperty("request_id", Required = Required.Always)]
    public long RequestID { get; set; }
}
