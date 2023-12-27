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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер для управления связями между внутренними и внешними продуктами поставщиков.
/// </summary>
[Authorize]
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
    [HttpGet]
    public IActionResult List()
    {
        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        var orders = _orderRepository.GetEntities()
            .Where(x => x.Creator.Key == user.Key)
            .OrderBy(x => x.OrderDate)
            .ToList();

        return View(orders);
    }

    [HttpGet("orders/api/list")]
    public IReadOnlyCollection<Order> ApiList()
    {
        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return null!;
        }

        var orders = _orderRepository.GetEntities()
            .Where(x => x.Creator.Key == user.Key)
            .OrderBy(x => x.OrderDate)
            .ToList();

        return orders;
    }

    // GET: Orders/details
    /// <summary xml:lang = "ru">
    /// Возвращает форму с детальным описанием заказа.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    [HttpGet]
    public ActionResult Details(long key)
    {
        var order = _orderRepository.GetByKey(new(key));

        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        if (order is null || order.Creator.Key != user.Key)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        return View(order);
    }

    [HttpGet("orders/api/details/{key}")] 
    // по факту не нужен, данные из списка выше достать можно
    // но на всякий случай
    public Order ApiDetails(long key)
    {
        var order = _orderRepository.GetByKey(new(key));

        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return null;
        }

        if (order is null || order.Creator.Key != user.Key)
        {
            Response.StatusCode = 404;
            return null;
        }

        return order;
    }

    // GET: Orders/cancel
    /// <summary xml:lang = "ru">
    /// Отменяет заказ.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    [HttpGet("orders/cancel/{key}")]
    public async Task<ActionResult> CancelAsync([FromRoute] long key)
    {
        var order = _orderRepository.GetByKey(new(key))!;

        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        if (order is null || order.Creator.Key != user.Key)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        order.State = OrderState.Cancel;

        _orderRepository.UpdateState(order);
        _orderRepository.Save();

        await _wtCommandSender.SendAsync(new CancelTransactionRequestCommand(
            new ExecutableID(Guid.NewGuid().ToString()),
            order.Key));

        return RedirectToAction("List");
    }

    /// <summary>
    /// Возвращает форму на оплату заказа.
    /// </summary>
    /// <param name="orderId">Идентификатор заказа.</param>
    /// <returns></returns>
    [HttpGet("orders/pay/{orderId}")]
    public IActionResult Pay([FromRoute] long orderId) => View();

    /// <summary>
    /// Запрос на оплату заказа.
    /// </summary>
    /// <param name="orderId">Идентифкатор заказа.</param>
    /// <param name="model">Платежные данные.</param>
    /// <returns></returns>
    [HttpPost(("orders/pay/{orderId}"))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PayAsync([FromRoute] long orderId, OrderPayModel model)
    {
        var order = _orderRepository.GetByKey(new(orderId));

        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        if (order is null || order.Creator.Key != user.Key)
        {
            Response.StatusCode = 404;
            return NotFound();
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

    [HttpPost(("orders/api/pay/{orderId}"))]
    public async Task<IActionResult> ApiPayAsync([FromRoute] long orderId, [FromBody] OrderPayModel model)
    {
        var order = _orderRepository.GetByKey(new(orderId));

        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        if (order is null || order.Creator.Key != user.Key)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        var transactions = new List<Transaction>();

        foreach (var item in order.Items.GroupBy(x => x.Product.Provider))
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

    /// <summary>
    /// Запрос на заказов требующих обработки.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult Aprove()
    {
        var ordersWithWaitStatus = _orderRepository.GetEntities()
            .Where(x => x.State is OrderState.ProductDeliveryWait or OrderState.Ready);

        return View(ordersWithWaitStatus);
    }

    [HttpGet("orders/api/aprove")]
    [Authorize(Policy = "OnlyForManager")]
    public IEnumerable<Order> ApiAprove()
    {
        var ordersWithWaitStatus = _orderRepository.GetEntities()
            .Where(x => x.State is OrderState.ProductDeliveryWait or OrderState.Ready);

        return ordersWithWaitStatus;
    }

    /// <summary>
    /// Запрос на получение заказа.
    /// </summary>
    /// <param name="id">Идентификатор заказа.</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult Ready([FromRoute] long id)
    {
        var order = _orderRepository.GetByKey(new(id));

        if (order is null || order.State is not OrderState.ProductDeliveryWait)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        order.State = OrderState.Ready;

        _orderRepository.UpdateState(order);
        _orderRepository.Save();

        return RedirectToAction("Aprove");
    }

    /// <summary>
    /// Запрос на получение заказа.
    /// </summary>
    /// <param name="id">Идентифкатор заказа.</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult Receive([FromRoute] long id)
    {
        var order = _orderRepository.GetByKey(new(id));

        if (order is null || order.State is not OrderState.Ready)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        order.State = OrderState.Received;

        _orderRepository.UpdateState(order);
        _orderRepository.Save();

        return RedirectToAction("Aprove");
    }

    private User? GetCurrentUser()
    {
        if (User.Identity is null || User.Identity.Name is null)
            return null;

        return _userRepository.GetByEmail(User.Identity.Name);
    }
}
