using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Commands.WT;
using Market.Logic.Transport.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Market.Logic.Transport.Serializers;

using TCancelCommand = Models.Commands.WT.CancelTransactionRequestCommand;
using TCommandBase = CommandBase;
using TCreateCommand = Models.Commands.WT.CreateTransactionRequestCommand;
using TFinishCommand = Models.Commands.WT.FinishTransactionRequestCommand;
using TRefundCommand = Models.Commands.WT.RefundTransactionRequestCommand;

/// <summary xml:lang = "ru">
/// Сериализатор команд для сервиса WT.
/// </summary>
public sealed class WTCommandSerializer : ISerializer<WTCommand, string>
{
    /// <inheritdoc/>
    public string Serialize(WTCommand source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        TCommandBase transportCommand = source switch
        {
            CreateTransactionRequestCommand createCommand => TCreateCommand.ToModel(createCommand),
            FinishTransactionRequestCommand finishCommand => TFinishCommand.ToModel(finishCommand),
            RefundTransactionRequestCommand refundCommand => TRefundCommand.ToModel(refundCommand),
            CancelTransactionRequestCommand cancelCommand => TCancelCommand.ToModel(cancelCommand),
            var unknownCommandType =>
                throw new InvalidOperationException($"The source contains unknown command '{unknownCommandType?.GetType().Name}'. ")
        };

        return JsonConvert.SerializeObject(transportCommand, new StringEnumConverter());
    }
}
