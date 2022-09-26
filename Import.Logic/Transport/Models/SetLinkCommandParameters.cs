using Newtonsoft.Json;

namespace Import.Logic.Transport.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель параметров для установки связи.
/// </summary>
public sealed class SetLinkCommandParameters
{
    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    [JsonProperty("internal_id")]
    public long InternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    [JsonProperty("internal_id")]
    public long ExternalID { get; set; }

    /// <summary xml:lang = "ru">
    /// Идентификатор провайдера.
    /// </summary>
    [JsonProperty("provider_id")]
    public long ProviderID { get; set; }
}