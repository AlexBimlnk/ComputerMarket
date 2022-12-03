using General.Logic;
using General.Storage;
using General.Transport;

using Market.Logic.Commands.Import;
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

    private static async Task HandleCancelledRequestAsync(Order order)
    {
        if (order.State is OrderState.PaymentWait or OrderState.ProviderAnswerWait)
        {
            order.State = OrderState.Cancel;
            // ToDo: Полный возврат средств.
        }

        else
        {
            order.State = OrderState.Cancel;
            // ToDo: Неполный возврат средств. Доделаем вместе с командами.
        }

        // Наверное здесь удалять заказы не нужно. А обновить его состояние. А удалять их по старости
        // в бд тригером
    }

    private static async Task UpdateOrderStateAsync(Order order, OrderState newState)
    {
        order.State = newState;
        // await _repo.Update
        return;
    }

    private static async Task HandleAbortedOrderAsync(Order order)
    {
        // Обработка неудачной транзакции.
        // По идее мы просто должны оповестить об этом пользователя.
        // ToDo
        order.State = OrderState.PaymentWait;
        return;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(string request, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(request))
            throw new ArgumentException("Source can't be null or empty or contains only whitespaces.");

        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Processing new request");

        var transationRequestResult = _deserializer.Deserialize(request)!;

        var linkedOrder = _orderRepository.GetByKey(transationRequestResult.TransactionRequestId);

        if (linkedOrder is null)
            throw new InvalidOperationException($"Order id: {transationRequestResult.TransactionRequestId} is not exist");

        var handleTask = transationRequestResult switch
        {
            { IsCancelled: true } n => HandleCancelledRequestAsync(linkedOrder),
            { State: TransactionRequestState.Held } => UpdateOrderStateAsync(linkedOrder, OrderState.ProviderAnswerWait),
            { State: TransactionRequestState.Finished } => UpdateOrderStateAsync(linkedOrder, OrderState.ProductDeliveryWait),
            { State: TransactionRequestState.Aborted } => HandleAbortedOrderAsync(linkedOrder),
            _ => Task.CompletedTask // Другие состояния нам не интересны.
        };

        await handleTask;

        _logger.LogInformation("Request processing complete");
    }
}
