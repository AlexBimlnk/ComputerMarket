using General.Storage;

using Market.Logic;
using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

namespace WalletTransaction;

/// <summary xml:lang = "ru">
/// Фоновый сервис, занимающийся обработкой протухания неоплаченных заказов.
/// </summary>
public sealed class OrderCancelService : BackgroundService
{
    private readonly ILogger<OrderCancelService> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _delayTime = TimeSpan.FromMinutes(1);
    private readonly TimeSpan _timeoutAwait = TimeSpan.FromMinutes(15);

    /// <summary xml:lang = "ru">
    /// Создает новый объект типа <see cref="OrderCancelService"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="applicationLifetime" xml:lang = "ru">
    /// Жизненный цикл приложения.
    /// </param>
    /// <param name="serviceScopeFactory" xml:lang = "ru">
    /// Фабрика служб.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public OrderCancelService(
        ILogger<OrderCancelService> logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_delayTime);

            _logger.LogDebug("Starting service...");

            using var scope = _serviceScopeFactory.CreateScope();

            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

            var checkableOrder = orderRepository.GetEntities()
                .Where(x => x.State is OrderState.PaymentWait);

            foreach (var order in checkableOrder)
            {
                if (DateTime.Now > order.OrderDate + _timeoutAwait)
                {
                    _logger.LogInformation(
                        "Order id: {OrderId} exceeded the payment waiting limit and will be closed",
                        order.Key);

                    order.State = OrderState.Cancel;
                    orderRepository.UpdateState(order);
                    orderRepository.Save();
                }
            }

            _logger.LogDebug("Stopping service...");
        }

        _applicationLifetime.StopApplication();
    }
}
