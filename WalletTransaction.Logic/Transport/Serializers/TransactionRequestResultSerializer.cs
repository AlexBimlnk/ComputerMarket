using General.Transport;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WalletTransaction.Logic.Transport.Models;

namespace WalletTransaction.Logic.Transport.Serializers;

/// <summary xml:lang = "ru">
/// Сериализатор результатов запросов на проведение транзакций.
/// </summary>
public sealed class TransactionRequestResultSerializer : ISerializer<ITransactionsRequest, string>
{
    /// <inheritdoc/>
    public string Serialize(ITransactionsRequest source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var result = new TransactionRequestResult
        {
            Id = source.Id.Value,
            IsCancelled = source.IsCancelled,
            LastState = source.CurrentState
        };

        return JsonConvert.SerializeObject(result, new StringEnumConverter());
    }
}
