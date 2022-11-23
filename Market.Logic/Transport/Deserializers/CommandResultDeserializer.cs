using General.Logic.Executables;
using General.Transport;

using Market.Logic.Commands;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Deserializers;

/// <summary xml:lang = "ru">
/// Десериализатор результат выполнения команд.
/// </summary>
public sealed class CommandResultDeserializer : IDeserializer<string, CommandResult>
{
    private JsonSerializer _serializer = JsonSerializer.CreateDefault();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="source"/> имеет некоректный формат.
    /// </exception>
    /// <exception cref="InvalidOperationException" xml:lang = "ru">
    /// Если json структура в <paramref name="source"/> имееет неверную структуру.
    /// </exception>
    public CommandResult Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        var result = _serializer.Deserialize<TCommandResult>(reader)!;

        if (result is null)
        {
            throw new InvalidOperationException("Give JSON file doesn't have correct structure.");
        }

        return new CommandResult(new ExecutableID(result.Id.Value), result.ErrorMessage);
    }

    private sealed class TCommandResult
    {
        [JsonProperty("id", Required = Required.Always)]
        public TCommandId Id { get; set; } = default!;

        [JsonProperty("isSuccess", Required = Required.Always)]
        public bool IsSuccess { get; set; }

        [JsonProperty("errorMessage", Required = Required.AllowNull)]
        public string? ErrorMessage { get; set; }
    }

    private sealed class TCommandId
    {
        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; } = default!;
    }
}


