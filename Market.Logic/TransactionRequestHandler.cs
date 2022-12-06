using General.Logic;
using General.Logic.Executables;
using General.Storage;
using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Commands.WT;
using Market.Logic.Markers;
using Market.Logic.Models;
using Market.Logic.Models.WT;
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
    private readonly IKeyableRepository<Order, ID> _orderRepository;

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
        IKeyableRepository<Order, ID> orderRepository,
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

        // Наверное здесь удалять заказы не нужно. А обновить его состояние.
        // А удалять их по старости в бд тригером

        if (result.State is TransactionRequestState.WaitHandle or TransactionRequestState.Aborted)
        {
            linkedOrder.State = OrderState.Cancel;
            // ToDo: UPdate order or delete order
        }

        else
        {
            linkedOrder.State = OrderState.Cancel;
            // Todo: Update or delete

            await _commandSender.SendAsync(new RefundTransactionRequestCommand(
                new ExecutableID(Guid.NewGuid().ToString()),
                result.TransactionRequestId));
        }
    }

    private async Task HandleHeldRequestStateAsync(TransactionRequestResult result)
    {
        var linkedOrder = _orderRepository.GetByKey(result.TransactionRequestId);

        if (linkedOrder is null)
            throw new InvalidOperationException($"Order id: {result.TransactionRequestId} is not exist");

        linkedOrder.State = OrderState.ProviderAnswerWait;
        // await _repo.Update
        return;
    }

    private async Task HandleAbortedRequestAsync(TransactionRequestResult result)
    {
        var linkedOrder = _orderRepository.GetByKey(result.TransactionRequestId);

        if (linkedOrder is null)
            throw new InvalidOperationException($"Order id: {result.TransactionRequestId} is not exist");

        // Обработка неудачной транзакции.
        // По идее мы просто должны оповестить об этом пользователя.
        // ToDo: оповещение о неудачной транзакции, возможно просто добавить новое состояние заказа
        linkedOrder.State = OrderState.PaymentWait;
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
