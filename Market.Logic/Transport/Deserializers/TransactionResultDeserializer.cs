using General.Transport;

using Market.Logic.Models.WT;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Deserializers;

/// <summary xml:lang = "ru">
/// Дессериализатор <see cref="TransactionRequestResult"/>.
/// </summary>
public sealed class TransactionResultDeserializer : IDeserializer<string, TransactionRequestResult>
{
    private JsonSerializer _serializer = JsonSerializer.CreateDefault();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="source"/> имел неверный формат.
    /// </exception>
    public TransactionRequestResult Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        var result = _serializer.Deserialize<TransportTransactionRequestResult>(reader)!;

        return new TransactionRequestResult(
            new ID(result.TransactionRequestId),
            result.IsCancelled,
            result.State);
    }

    private sealed class TransportTransactionRequestResult
    {
        [JsonProperty("id", Required = Required.Always)]
        public long TransactionRequestId { get; set; }

        [JsonProperty("is_cancelled", Required = Required.Always)]
        public bool IsCancelled { get; set; }

        [JsonProperty("last_state", Required = Required.Always)]
        public TransactionRequestState State { get; set; }
    }
}