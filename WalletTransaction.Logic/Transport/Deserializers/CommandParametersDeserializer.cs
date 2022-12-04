using General.Logic.Commands;
using General.Transport;

using Newtonsoft.Json;

using WalletTransaction.Logic.Commands;
using WalletTransaction.Logic.Transport.Models.Commands;

namespace WalletTransaction.Logic.Transport.Deserializers;

using TCommand = Models.Commands.CommandBase;

/// <summary xml:lang = "ru">
/// Дессериализатор из <see cref="string"/> в <see cref="CommandParametersBase"/>.
/// </summary>
public sealed class CommandParametersDeserializer : IDeserializer<string, CommandParametersBase>
{
    private readonly JsonSerializer _serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
    {
        Converters = new[]
        {
            new CommandConverter()
        }
    });

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="source"/> имел неверный формат.
    /// </exception>
    public CommandParametersBase Deserialize(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException(
                "The source is null, has only whitespaces or empty.",
                nameof(source));

        using var reader = new JsonTextReader(new StringReader(source));

        TCommand trasnportCommand = _serializer.Deserialize<TCommand>(reader)!;

        return trasnportCommand switch
        {
            CreateRequestCommand createRequestCommand => new CreateTransactionRequestCommandParameters(
                new(createRequestCommand.Id),
                new TransactionRequest(
                    new(createRequestCommand.TransactionRequest.Id),
                    createRequestCommand.TransactionRequest.Transactions
                        .Select(x => Models.Transaction.FromModel(x))
                        .ToList())),

            CancelRequestCommand cancelCommand => new CancelTransactionRequestCommandParameters(
                new(cancelCommand.Id),
                new(cancelCommand.RequestID)),

            FinishRequestCommand finishCommand => new FinishTransactionRequestCommandParameters(
                new(finishCommand.Id),
                new(finishCommand.RequestID)),

            RefundRequestCommand finishCommand => new RefundTransactionRequestCommandParameters(
                new(finishCommand.Id),
                new(finishCommand.RequestID)),

            var unknownCommandType =>
                throw new InvalidOperationException($"The source contains unknown command '{unknownCommandType?.GetType().Name}'. ")
        };
    }
}
