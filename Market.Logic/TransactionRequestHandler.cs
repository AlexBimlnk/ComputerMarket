using General.Logic;
using General.Logic.Executables;
using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Commands.WT;
using Market.Logic.Markers;
using Market.Logic.Models;
using Market.Logic.Models.WT;
using Market.Logic.Storage.Repositories;
using Market.Logic.Transport.Configurations;

using Microsoft.Extensions.Logging;

namespace Market.Logic;

/// <summary xml:lang = "ru">
/// Обработчки информации о транзакциях от сервиса WT.
/// </summary>
public sealed class TransactionRequestHandler : IAPIRequestHandler<WTMarker>
{
    private readonly ILogger<TransactionRequestHandler> _logger;
    private readonly IDeserializer<string, TransactionRequestResult> _deserializer;
    private readonly ISender<WTCommandConfigurationSender, WTCommand> _commandSender;
    private readonly IOrderRepository _orderRepository;

    /// <summary xml:lang = "ru">
    /// Создаёт экземлпяр класса <see cref="TransactionRequestHandler"/>.
    /// </summary>
    /// <param name="deserializer" xml:lang = "ru">Десериализатор продуктов.</param>
    /// <param name="orderRepository" xml:lang = "ru">Репозиторий заказов.</param>
    /// <param name="commandSender" xml:lang = "ru">Отправитель команд в сервис WT.</param>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    /// <exception cref="ArgumentNullException">
    /// Если один из параметров - <see langword="null"/>.
    /// </exception>
    public TransactionRequestHandler(
        IDeserializer<string, TransactionRequestResult> deserializer,
        IOrderRepository orderRepository,
        ISender<WTCommandConfigurationSender, WTCommand> commandSender,
        ILogger<TransactionRequestHandler> logger)
    {
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _commandSender = commandSender ?? throw new ArgumentNullException(nameof(commandSender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private async Task HandleCancelledRequestAsync(TransactionRequestResult result)
    {
        var linkedOrder = _orderRepository.GetByKey(result.TransactionRequestId);

        if (linkedOrder is null)
            throw new InvalidOperationException($"Order id: {result.TransactionRequestId} is not exist");

        if (result.State is TransactionRequestState.WaitHandle or TransactionRequestState.Aborted)
        {
            linkedOrder.State = OrderState.Cancel;
            _orderRepository.UpdateState(linkedOrder);
            _orderRepository.Save();
        }

        else
        {
            linkedOrder.State = OrderState.Cancel;
            _orderRepository.UpdateState(linkedOrder);
            _orderRepository.Save();

            await _commandSender.SendAsync(new RefundTransactionRequestCommand(
                new ExecutableID(Guid.NewGuid().ToString()),
                result.TransactionRequestId));
        }
    }

    private Task HandleHeldRequestStateAsync(TransactionRequestResult result)
    {
        var linkedOrder = _orderRepository.GetByKey(result.TransactionRequestId);

        if (linkedOrder is null)
            throw new InvalidOperationException($"Order id: {result.TransactionRequestId} is not exist");

        linkedOrder.State = OrderState.ProviderAnswerWait;
        _orderRepository.UpdateState(linkedOrder);
        _orderRepository.Save();
        return Task.CompletedTask;
    }

    private Task HandleAbortedRequestAsync(TransactionRequestResult result)
    {
        var linkedOrder = _orderRepository.GetByKey(result.TransactionRequestId);

        if (linkedOrder is null)
            throw new InvalidOperationException($"Order id: {result.TransactionRequestId} is not exist");

        linkedOrder.State = OrderState.PaymentError;
        _orderRepository.UpdateState(linkedOrder);
        _orderRepository.Save();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(string request, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(request))
            throw new ArgumentException("Source can't be null or empty or contains only whitespaces.");

        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Processing new request");

        var transationRequestResult = _deserializer.Deserialize(request)!;

        var handleTask = transationRequestResult switch
        {
            { IsCancelled: true } n => HandleCancelledRequestAsync(transationRequestResult),
            { State: TransactionRequestState.Held } => HandleHeldRequestStateAsync(transationRequestResult),
            { State: TransactionRequestState.Aborted } => HandleAbortedRequestAsync(transationRequestResult),
            _ => Task.CompletedTask // Другие состояния нам не интересны.
        };

        await handleTask;

        _logger.LogInformation("Request processing complete");
    }
}
