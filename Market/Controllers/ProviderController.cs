using System.Globalization;
using System.Net;
using System.Net.WebSockets;

using Market.Logic;
using Market.Logic.Models;
using Market.Logic.Storage.Repositories;
using Market.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

/// <summary>
/// Контроллер для управления провайдерами.
/// </summary>
[Authorize]
public class ProviderController : Controller
{
    private readonly IProvidersRepository _providerRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IOrderRepository _orderRepositoty;
    private readonly ILogger<ProviderController> _logger;

    /// <summary>
    /// Создаёт экземпляр класса <see cref="ProviderController"/>.
    /// </summary>
    /// <param name="providerRepository">Репозиторий провайдеров.</param>
    /// <param name="usersRepository">Репозиторий пользователей.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="orderRepository">Репозиторий заказов.</param>
    public ProviderController(IProvidersRepository providerRepository,
        IUsersRepository usersRepository,
        IOrderRepository orderRepository,
        ILogger<ProviderController> logger)
    {
        _providerRepository = providerRepository;
        _usersRepository = usersRepository;
        _orderRepositoty = orderRepository;
        _logger = logger;
    }

    /// <summary>
    /// Возвращает фору для регистрации провайдера.
    /// </summary>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View();

    /// <summary>
    /// Запрос на регистрацию провайдера.
    /// </summary>
    /// <param name="model">
    /// Информация о новом провайдере.
    /// </param>
    /// <returns><see cref="Task{TResult}"/></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(ProviderRegisterViewModel model)
    {
        var provider = new Provider(default, model.Name, new Margin(1.0m), new PaymentTransactionsInformation(model.INN, model.BankAccount));

        await _providerRepository.AddAsync(provider);
        _providerRepository.Save();

        return RedirectToAction("List");
    }

    [HttpPost("provider/api/register")]
    [AllowAnonymous]
    public async Task<IActionResult> ApiRegisterAsync([FromBody] ProviderRegisterViewModel model)
    {
        var provider = new Provider(
            default, 
            model.Name,
            new Margin(1.0m),
            new PaymentTransactionsInformation(model.INN, model.BankAccount));

        await _providerRepository.AddAsync(provider);
        _providerRepository.Save();

        return RedirectToAction("List");
    }

    /// <summary xml:lang = "ru">
    /// Возвращает список провайдеров.
    /// </summary>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    [HttpGet]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult ListAsync() => View(_providerRepository.GetEntities());

    [HttpGet("provider/api/list")]
    [Authorize(Policy = "OnlyForManager")]
    public IEnumerable<Provider> ApiListAsync() => _providerRepository.GetEntities();

    /// <summary xml:lang = "ru">
    /// Возвращает форму для редактирования информации о поставщиках.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    [HttpGet]
    [Authorize(Policy = "OnlyForManager")]
    public ActionResult Edit(long id)
    {
        var provider = _providerRepository.GetByKey(new(id));

        if (provider is null)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        return View(new ManageProviderViewModel()
        {
            Name = provider.Name,
            Key = provider.Key.Value,
            INN = provider.PaymentTransactionsInformation.INN,
            BankAccount = provider.PaymentTransactionsInformation.BankAccount,
            Margin = provider.Margin.Value.ToString(CultureInfo.CurrentCulture),
            IsAproved = provider.IsAproved
        });
    }

    [HttpGet("provider/api/{id}")]
    [Authorize(Policy = "OnlyForManager")]
    public ManageProviderViewModel ApiEdit([FromRoute] long id)
    {
        var provider = _providerRepository.GetByKey(new(id));

        if (provider is null)
        {
            Response.StatusCode = 404;
            return null;
        }

        return new ManageProviderViewModel()
        {
            Name = provider.Name,
            Key = provider.Key.Value,
            INN = provider.PaymentTransactionsInformation.INN,
            BankAccount = provider.PaymentTransactionsInformation.BankAccount,
            Margin = provider.Margin.Value.ToString(CultureInfo.CurrentCulture),
            IsAproved = provider.IsAproved
        };
    }

    /// <summary xml:lang = "ru">
    /// Запрос на изменение провайдера.
    /// </summary>
    /// <param name="link" xml:lang = "ru"> 
    /// Провайдер с новыми данными. 
    /// </param>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult Edit(ManageProviderViewModel provider)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var domainProvider = _providerRepository.GetByKey(new(provider.Key));

        if (domainProvider is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        if (!decimal.TryParse(provider.Margin, out var margin))
        {
            ModelState.AddModelError("Margin", "Margin is not correct");
            return View();
        }

        var updatedProvider = new Provider(
            domainProvider.Key,
            domainProvider.Name,
            new Margin(margin),
            domainProvider.PaymentTransactionsInformation);

        updatedProvider.IsAproved = domainProvider.IsAproved;

        _providerRepository.Update(updatedProvider);
        _providerRepository.Save();

        return RedirectToAction("List");
    }

    [HttpPost("provider/api/edit")]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult ApiEdit([FromBody] ManageProviderViewModel provider)
    {
        var domainProvider = _providerRepository.GetByKey(new(provider.Key));

        if (domainProvider is null)
        {
            Response.StatusCode = 400;
            return null;
        }

        if (!decimal.TryParse(provider.Margin, out var margin))
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return null!;
        }

        var updatedProvider = new Provider(
            domainProvider.Key,
            domainProvider.Name,
            new Margin(margin),
            domainProvider.PaymentTransactionsInformation);

        updatedProvider.IsAproved = domainProvider.IsAproved;

        _providerRepository.Update(updatedProvider);
        _providerRepository.Save();

        return RedirectToAction("List");
    }

    /// <summary xml:lang = "ru">
    /// Запрос на изменение провайдера.
    /// </summary>
    /// <param name="link" xml:lang = "ru"> 
    /// Провайдер с новыми данными. 
    /// </param>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult Aprove(ManageProviderViewModel provider)
    {
        var domainProvider = _providerRepository.GetByKey(new(provider.Key));

        if (domainProvider is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        if (!decimal.TryParse(provider.Margin, out var margin))
        {
            ModelState.AddModelError("Margin", "Margin is not correct");
            return View();
        }

        if (domainProvider.IsAproved == false)
        {
            var updatedProvider = new Provider(
                domainProvider.Key,
                domainProvider.Name,
                new Margin(margin),
                domainProvider.PaymentTransactionsInformation);

            updatedProvider.IsAproved = true;

            _providerRepository.Update(updatedProvider);
            _providerRepository.Save();
        }

        return RedirectToAction("List");
    }

    [HttpPost("provider/api/aprove")]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult ApiAprove([FromBody] ManageProviderViewModel provider)
    {
        var domainProvider = _providerRepository.GetByKey(new(provider.Key));

        if (domainProvider is null)
        {
            Response.StatusCode = 400;
            return null;
        }

        if (!decimal.TryParse(provider.Margin, out var margin))
        {
            return null;
        }

        if (domainProvider.IsAproved == false)
        {
            var updatedProvider = new Provider(
                domainProvider.Key,
                domainProvider.Name,
                new Margin(margin),
                domainProvider.PaymentTransactionsInformation);

            updatedProvider.IsAproved = true;

            _providerRepository.Update(updatedProvider);
            _providerRepository.Save();
        }

        return RedirectToAction("List");
    }

    /// <summary>
    /// Возвращает список всех агентов провайдера.
    /// </summary>
    /// <param name="providerId">Идентификатор провайдера.</param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpGet("provider/agents/{providerId}")]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult Agents([FromRoute] long providerId)
    {
        var provider = _providerRepository.GetByKey(new(providerId));

        if (provider is null || !provider.IsAproved)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        ViewBag.ProviderId = providerId;
        var agents = _providerRepository.GetAgents(provider);

        return View(agents);
    }

    [HttpGet("provider/api/agents/{providerId}")]
    [Authorize(Policy = "OnlyForManager")]
    public IEnumerable<User> ApiAgents([FromRoute] long providerId)
    {
        var provider = _providerRepository.GetByKey(new(providerId));

        if (provider is null || !provider.IsAproved)
        {
            Response.StatusCode = 400;
            return null!;
        }

        ViewBag.ProviderId = providerId;
        var agents = _providerRepository.GetAgents(provider);

        return agents;
    }

    /// <summary>
    /// Запрос на добавления представителя провайдера.
    /// </summary>
    /// <param name="providerId">Индетификатор провайдера.</param>
    /// <param name="model">Информация о представителе.</param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPost("provider/agents/{providerId}/add")]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult AddAgent([FromRoute] long providerId, NewAgentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var provider = _providerRepository.GetByKey(new(providerId));
        var user = _usersRepository.GetByEmail(model.Email);

        if (provider is null || !provider.IsAproved)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        if (user is null)
        {
            ModelState.AddModelError("Email", "User with this email not exsist");
            return View();
        }

        if (user.Type != UserType.Customer)
        {
            ModelState.AddModelError("Email", "Incorrect user");
            return View();
        }

        user.Type = UserType.Agent;

        _usersRepository.Update(user);

        _usersRepository.Save();

        _providerRepository.AddAgent(new ProviderAgent(user, provider));

        _providerRepository.Save();

        return RedirectToAction("Agents", new { ProviderId = providerId });
    }

    [HttpPost("provider/api/agents/{providerId}/add")]
    [Authorize(Policy = "OnlyForManager")]
    public IEnumerable<User> ApiAddAgent([FromRoute] long providerId, [FromBody] NewAgentViewModel model)
    {
        var provider = _providerRepository.GetByKey(new(providerId));
        var user = _usersRepository.GetByEmail(model.Email);

        if (provider is null || !provider.IsAproved)
        {
            Response.StatusCode = 400;
            return null!;
        }

        if (user is null)
        {
            Response.StatusCode = 400;
            return null;
        }

        if (user.Type != UserType.Customer)
        {
            Response.StatusCode = 400;
            return null;
        }

        user.Type = UserType.Agent;

        _usersRepository.Update(user);

        _usersRepository.Save();

        _providerRepository.AddAgent(new ProviderAgent(user, provider));

        _providerRepository.Save();

        return ApiAgents(providerId);
    }

    /// <summary>
    /// Возвращает форму на добавление нового представителя провайдера.
    /// </summary>
    /// <param name="providerId">Индетификтор провайдера</param>
    /// <returns></returns>
    [HttpGet("provider/agents/{providerId}/add")]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult AddAgent([FromRoute] long providerId)
    {
        var provider = _providerRepository.GetByKey(new(providerId));

        if (provider is null || !provider.IsAproved)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }
        return View();
    }

    [HttpGet("provider/api/agents/{providerId}/add")]
    [Authorize(Policy = "OnlyForManager")]
    public Provider ApiAddAgent([FromRoute] long providerId)
    {
        var provider = _providerRepository.GetByKey(new(providerId));

        if (provider is null || !provider.IsAproved)
        {
            Response.StatusCode = 400;
            return null;
        }
        return provider;
    }

    /// <summary>
    /// Удаляет представителя провайдера.
    /// </summary>
    /// <param name="providerId">Индетификатор провайдера.</param>
    /// <param name="userId">Индетификатор пользователя.</param>
    /// <returns></returns>
    [HttpGet("provider/agents/{providerId}/remove/{userId}")]
    [Authorize(Policy = "OnlyForManager")]
    public IActionResult RemoveAgent([FromRoute] long providerId, [FromRoute] long userId)
    {
        var user = _usersRepository.GetByKey(new(userId));
        var provider = _providerRepository.GetByKey(new(providerId));

        if (user is null || provider is null || !provider.IsAproved)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        var agent = new ProviderAgent(user, provider);

        user.Type = UserType.Customer;

        _providerRepository.RemoveAgent(agent);
        _providerRepository.Save();

        _usersRepository.Update(user);
        _usersRepository.Save();

        return RedirectToAction("Agents", new { ProviderId = providerId });
    }

    [HttpGet("provider/api/agents/{providerId}/remove/{userId}")]
    [Authorize(Policy = "OnlyForManager")]
    public IEnumerable<User> ApiRemoveAgent([FromRoute] long providerId, [FromRoute] long userId)
    {
        var user = _usersRepository.GetByKey(new(userId));
        var provider = _providerRepository.GetByKey(new(providerId));

        if (user is null || provider is null || !provider.IsAproved)
        {
            Response.StatusCode = 400;
            return null!;
        }

        var agent = new ProviderAgent(user, provider);

        user.Type = UserType.Customer;

        _providerRepository.RemoveAgent(agent);
        _providerRepository.Save();

        _usersRepository.Update(user);
        _usersRepository.Save();

        return ApiAgents(providerId);
    }

    /// <summary>
    /// Запрос на список заказов на поставщика, чьим представителем является пользователь.
    /// </summary>
    /// <returns>Представление со списком заказов.</returns>
    [HttpGet]
    [Authorize(Policy = "OnlyForAgents")]
    public IActionResult Orders()
    {
        var user = GetCurrentUser();

        var agent = _providerRepository.GetAgent(user!);

        if (agent is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }
        ViewData["ProviderKey"] = agent.Provider.Key.Value;

        var orders = _orderRepositoty.GetProviderOrders(agent.Provider);

        return View(orders);
    }

    [HttpGet("provider/api/orders")]
    [Authorize(Policy = "OnlyForAgents")]
    public IEnumerable<Order> ApiOrders()
    {
        var user = GetCurrentUser();

        var agent = _providerRepository.GetAgent(user!);

        if (agent is null)
        {
            Response.StatusCode = 400;
            return null;
        }

        var orders = _orderRepositoty.GetProviderOrders(agent.Provider);

        return orders;
    }

    /// <summary>
    /// Детали по заказу.
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>Представление с информацией по заказу.</returns>
    [HttpGet("provider/orders/details/{id}")]
    [Authorize(Policy = "OnlyForAgents")]
    public IActionResult Details(long id)
    {
        var user = GetCurrentUser();

        var agent = _providerRepository.GetAgent(user!)!;

        var order = _orderRepositoty.GetProviderOrders(agent.Provider).SingleOrDefault(x => x.Key.Value == id);

        if (order is null)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        return View(order.Items.Where(x => x.Product.Provider.Key == agent.Provider.Key));
    }

    [HttpGet("provider/api/orders/details/{id}")]
    [Authorize(Policy = "OnlyForAgents")]
    public IEnumerable<PurchasableEntity> ApiDetails([FromRoute] long id)
    {
        var user = GetCurrentUser();

        var agent = _providerRepository.GetAgent(user!)!;

        var order = _orderRepositoty.GetProviderOrders(agent.Provider).SingleOrDefault(x => x.Key.Value == id);

        if (order is null)
        {
            Response.StatusCode = 404;
            return null;
        }

        return order.Items.Where(x => x.Product.Provider.Key == agent.Provider.Key);
    }

    /// <summary>
    /// Запрос на подтверждение заказа от поставщика.
    /// </summary>
    /// <param name="id">Идентификатор заказа.</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "OnlyForAgents")]
    public IActionResult Ready(long id)
    {
        var user = GetCurrentUser();

        var agent = _providerRepository.GetAgent(user!)!;

        var order = _orderRepositoty.GetProviderOrders(agent.Provider).SingleOrDefault(x => x.Key.Value == id);

        if (order is null)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        _orderRepositoty.ProviderArpove(order, agent.Provider, true);
        _orderRepositoty.Save();

        return RedirectToAction("Orders");
    }

    [HttpGet("provider/api/ready/{id}")]
    [Authorize(Policy = "OnlyForAgents")]
    public IEnumerable<Order> ApiReady([FromRoute] long id)
    {
        var user = GetCurrentUser();

        var agent = _providerRepository.GetAgent(user!)!;

        var order = _orderRepositoty.GetProviderOrders(agent.Provider)
            .SingleOrDefault(x => x.Key.Value == id);

        if (order is null)
        {
            Response.StatusCode = 404;
            return null;
        }

        _orderRepositoty.ProviderArpove(order, agent.Provider, true);
        _orderRepositoty.Save();

        return ApiOrders();
    }

    /// <summary>
    /// Запрос на отмену заказа от поставщика.
    /// </summary>
    /// <param name="id">Идентифкатор заказа.</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "OnlyForAgents")]
    public IActionResult Decline(long id)
    {
        var user = GetCurrentUser();

        var agent = _providerRepository.GetAgent(user!)!;

        var order = _orderRepositoty.GetProviderOrders(agent.Provider).SingleOrDefault(x => x.Key.Value == id);

        if (order is null)
        {
            Response.StatusCode = 404;
            return NotFound();
        }

        _orderRepositoty.ProviderArpove(order, agent.Provider, false);
        _orderRepositoty.Save();

        return RedirectToAction("Orders");
    }

    [HttpGet("provider/api/decline/{id}")]
    [Authorize(Policy = "OnlyForAgents")]
    public IEnumerable<Order> ApiDecline([FromRoute] long id)
    {
        var user = GetCurrentUser();

        var agent = _providerRepository.GetAgent(user!)!;

        var order = _orderRepositoty.GetProviderOrders(agent.Provider)
            .SingleOrDefault(x => x.Key.Value == id);

        if (order is null)
        {
            Response.StatusCode = 404;
            return null!;
        }

        _orderRepositoty.ProviderArpove(order, agent.Provider, false);
        _orderRepositoty.Save();

        return ApiOrders();
    }

    private User? GetCurrentUser()
    {
        if (User.Identity is null || User.Identity.Name is null)
            return null;

        return _usersRepository.GetByEmail(User.Identity.Name);
    }
}
