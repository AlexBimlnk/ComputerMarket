using Newtonsoft.Json;

namespace WalletTransaction.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Транспортная модель команды создания запроса.
/// </summary>
public sealed class CreateRequestCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CreateRequestCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public CreateRequestCommand(CommandType type, string id) : base(type, id) { }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    [JsonProperty("request", Required = Required.Always)]
    public TransactionRequest TransactionRequest { get; set; } = default!;
}