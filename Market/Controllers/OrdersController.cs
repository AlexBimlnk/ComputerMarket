using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

using General.Logic.Executables;
using General.Storage;
using General.Transport;

using Market.Logic;
using Market.Logic.Commands.Import;
using Market.Logic.Commands.WT;
using Market.Logic.Models;
using Market.Logic.Models.WT;
using Market.Logic.Storage.Repositories;
using Market.Logic.Transport.Configurations;
using Market.Models;

using Microsoft.AspNetCore.Mvc;

using NuGet.Protocol;

namespace Market.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер для управления связями между внутренними и внешними продуктами поставщиков.
/// </summary>
public sealed class OrdersController : Controller
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IUsersRepository _userRepository;
    private readonly ISender<WTCommandConfigurationSender, WTCommand> _wtCommandSender;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="OrdersController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="orderRepository" xml:lang = "ru">
    /// Хранилище заказов.
    /// </param>
    /// <param name="usersRepository" xml:lang = "ru">
    /// Хранилище пользователей.
    /// </param>
    /// <param name="wtCommandSender" xml:lang = "ru">
    /// Отправитель команд.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public OrdersController(
        ILogger<OrdersController> logger,
        IOrderRepository orderRepository,
        IUsersRepository usersRepository,
        ISender<WTCommandConfigurationSender, WTCommand> wtCommandSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _userRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        _wtCommandSender = wtCommandSender ?? throw new ArgumentNullException(nameof(wtCommandSender));
    }

    // Get: Orders/List
    /// <summary xml:lang = "ru">
    /// Возвращает список заказов.
    /// </summary>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    public IActionResult List()
    {
        var userEmail = HttpContext.User.Claims
            .Where(x => x.Type == ClaimsIdentity.DefaultNameClaimType)
            .Single()
            .Value;

        var user = _userRepository.GetByEmail(userEmail)!;

        var orders = _orderRepository.GetEntities()
            .Where(x => x.Creator.Key == user.Key)
            .OrderBy(x => x.OrderDate)
            .ToList();

        return View(orders);
    }

    // GET: Orders/details
    /// <summary xml:lang = "ru">
    /// Возвращает форму с детальным описанием заказа.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    public ActionResult Details(long key) => View(_orderRepository.GetByKey(new(key)));

    // GET: Orders/cancel
    /// <summary xml:lang = "ru">
    /// Отменяет заказ.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    public async Task<ActionResult> CancelAsync(long key)
    {
        var order = _orderRepository.GetByKey(new(key))!;

        order.State = OrderState.Cancel;

        _orderRepository.UpdateState(order);
        _orderRepository.Save();

        await _wtCommandSender.SendAsync(new CancelTransactionRequestCommand(
            new ExecutableID(Guid.NewGuid().ToString()),
            order.Key));

        return RedirectToAction("List");
    }

    [HttpGet("orders/pay/{orderId}")]
    public IActionResult Pay([FromRoute] long orderId) => View();

    [HttpPost(("orders/pay/{orderId}"))]
    public async Task<IActionResult> PayAsync([FromRoute] long orderId, OrderPayModel model)
    {
        var order = _orderRepository.GetByKey(new(orderId));

        if (order is null)
        {
            Response.StatusCode = 400;
            return View();
        }

        var transactions = new List<Transaction>();

        foreach(var item in order.Items.GroupBy(x => x.Product.Provider))
        {
            transactions.Add(new Transaction(
                model.Account, 
                item.Key.PaymentTransactionsInformation.BankAccount, 
                item.Sum(x => x.Product.FinalCost), 
                item.Sum(x => x.Product.FinalCost) - item.Sum(x => x.Product.ProviderCost)));
        }

        await _wtCommandSender.SendAsync(new CreateTransactionRequestCommand(new(Guid.NewGuid().ToString()), order.Key, transactions));
            
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Aprove()
    {
        var ordersWithWaitStatus = _orderRepository.GetEntities()
            .Where(x => x.State is OrderState.ProductDeliveryWait or OrderState.Ready);

        return View(ordersWithWaitStatus);
    }

    [HttpGet]
    public IActionResult Ready(long id)
    {
        var order = _orderRepository.GetByKey(new(id));

        if (order is null || order.State is not OrderState.ProductDeliveryWait)
        {
            Response.StatusCode = 400;
            return View();
        }

        order.State = OrderState.Ready;

        _orderRepository.UpdateState(order);
        _orderRepository.Save();

        return RedirectToAction("Aprove");
    }

    [HttpGet]
    public IActionResult Receive(long id)
    {
        var order = _orderRepository.GetByKey(new(id));

        if (order is null || order.State is not OrderState.Ready)
        {
            Response.StatusCode = 400;
            return View();
        }

        order.State = OrderState.Received;

        _orderRepository.UpdateState(order);
        _orderRepository.Save();

        return RedirectToAction("Aprove");
    }

    
}
