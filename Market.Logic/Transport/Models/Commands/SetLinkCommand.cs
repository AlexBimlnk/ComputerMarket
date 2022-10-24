using Market.Logic.Commands;
using Market.Logic.Commands.Import;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Models.Commands;

/// <summary xml:lang = "ru">
/// Транспортная модель команды для установки связи.
/// </summary>
public sealed class SetLinkCommand : CommandBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="SetLinkCommand"/>.
    /// </summary>
    /// <param name="type" xml:lang = "ru">
    /// Тип команды.
    /// </param>
    public SetLinkCommand(string id, CommandType type) : base(id, type) { }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    [JsonProperty("internal_id")]
    public long InternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    [JsonProperty("external_id")]
    public long ExternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Имя провайдера.
    /// </summary>
    [JsonProperty("provider")]
    public string Provider { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Конвертирует доменную модель в транспортную.
    /// </summary>
    /// <param name="command" xml:lang = "ru">
    /// Доменная модель команды.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Транспортная модель доменной команды типа <see cref="SetLinkCommand"/>.
    /// </returns>
    public static SetLinkCommand ToModel(Logic.Commands.Import.SetLinkCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new SetLinkCommand(command.Id.Value, command.Type)
        {
            InternalID = command.InternalItemId.Value,
            ExternalID = command.ExternalItemId.Value,
            Provider = ToSnakeCase(command.Provider.Name)
        };
    }

    private static string ToSnakeCase(string str)
        => string.Concat(
            str.Select((x, i) =>
            {
                if (char.IsWhiteSpace(x))
                    return string.Empty;

                if (i > 0 && i < str.Length - 1 &&
                    char.IsUpper(x) && !char.IsUpper(str[i - 1]))
                    return $"_{x}";

                return x.ToString();
            }))
            .ToLower();
}
