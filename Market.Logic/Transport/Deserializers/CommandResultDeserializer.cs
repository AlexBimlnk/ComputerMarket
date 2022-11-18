using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using General.Logic.Commands;
using General.Transport;

using Market.Logic.Commands;

using Newtonsoft.Json;

namespace Market.Logic.Transport.Deserializers;

/// <summary>
/// 
/// </summary>
public sealed class CommandResultDeserializer : IDeserializer<string, CommandResult>
{
    private JsonSerializer _serializer = JsonSerializer.CreateDefault();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
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

        return new CommandResult(new CommandID(result.Id.Value), result.ErrorMessage);
    }

    private sealed class TCommandResult
    {
        [JsonProperty("id")]
        public TCommandId Id { get; set; } = default!;

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }
    }

    private sealed class TCommandId
    {
        [JsonProperty("value")]
        public string Value { get; set; } = default!;
    }
}


