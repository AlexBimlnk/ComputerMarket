using General.Transport;

using Market.Logic;
using Market.Logic.Commands.Import;
using Market.Logic.Commands.WT;
using Market.Logic.Models;
using Market.Logic.Models.Abstractions;
using Market.Logic.Storage.Repositories;
using Market.Logic.Transport.Configurations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[Authorize]
public class BasketController : Controller
{
    private readonly IUsersRepository _usersRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICatalog _catalog;
    private readonly ISender<WTCommandConfigurationSender, WTCommand> _wtCommandSender;

    public BasketController(IBasketRepository basketRepository, 
        IUsersRepository usersRepository, 
        ICatalog catalog, 
        IOrderRepository orderRepository,
        ISender<WTCommandConfigurationSender, WTCommand> wtCommandSender)
    {
        _basketRepository = basketRepository;
        _usersRepository = usersRepository;
        _orderRepository = orderRepository;
        _catalog= catalog;
        _wtCommandSender = wtCommandSender;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return BadRequest();
        }

        var items = _basketRepository.GetAllBasketItems(user).OrderBy(x => x.Product.Item.Key.Value);

        return View(items);
    }

    [HttpGet("api/index")]
    public IEnumerable<PurchasableEntity> ApiIndex()
    {
        var user = GetCurrentUser();

        if (user is null)
        {
            Response.StatusCode = 400;
            return null!;
        }

        var items = _basketRepository.GetAllBasketItems(user).OrderBy(x => x.Product.Item.Key.Value);

        return items;
    }

    [HttpGet]
    public IActionResult Add(long providerId, long itemId)
    {
        var product = _catalog.GetProductByKey((new(providerId), new(itemId)));

        var user = GetCurrentUser();

        if (product is not null && user is not null)
        {
            _basketRepository.AddToBasketAsync(user, product);
        }

        _basketRepository.Save();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(long providerId, long itemId)
    {
        var product = _catalog.GetProductByKey((new(providerId), new(itemId)));

        var user = GetCurrentUser();

        if (product is not null && user is not null)
        {
            _basketRepository.DeleteFromBasket(user, product);
        }

        _basketRepository.Save();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Remove(long providerId, long itemId)
    {
        var product = _catalog.GetProductByKey((new(providerId), new(itemId)));

        var user = GetCurrentUser();

        if (product is not null && user is not null)
        {
            _basketRepository.RemoveFromBasket(user, product);
        }

        _basketRepository.Save();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrderAsync()
    {
        var selectedItems = GetItems(Request.Form["Selected"].ToString());

        var user = GetCurrentUser();

        if (user is null)
        {
            return RedirectToAction("Logout", "Account");
        }

        var items = _basketRepository.GetAllBasketItems(user);

        if (!items.Any())
        {
            return RedirectToAction("Index");
        }

        if (selectedItems.Any())
        {
            items = items.Join(selectedItems, x => (x.Product.Item.Key, x.Product.Provider.Key), y => y, (x, y) => x);
        }

        var order = new Order(default, user, items.ToHashSet());

        await _orderRepository.AddAsync(order);
        _orderRepository.Save();

        return RedirectToAction("List", "Orders");
    }

    [HttpPost("api/create_order")]
    public async Task<IActionResult> ApiCreateOrderAsync()
    {
        var selectedItems = GetItems(Request.Form["Selected"].ToString());

        var user = GetCurrentUser();

        if (user is null)
        {
            return RedirectToAction("Logout", "Account");
        }

        var items = _basketRepository.GetAllBasketItems(user);

        if (!items.Any())
        {
            return RedirectToAction("Index");
        }

        if (selectedItems.Any())
        {
            items = items.Join(selectedItems, x => (x.Product.Item.Key, x.Product.Provider.Key), y => y, (x, y) => x);
        }

        var order = new Order(default, user, items.ToHashSet());

        await _orderRepository.AddAsync(order);
        _orderRepository.Save();

        return RedirectToAction("List", "Orders");
    }

    private static IReadOnlySet<(ID, ID)> GetItems(string request)
    {
        var result = new HashSet<(ID, ID)>();

        foreach (var line in request.Split(','))
        {
            if (!line.Contains("_"))
            {
                continue;
            }

            var value = line.Split('_');
            long providerId = 0;
            var isSucces = long.TryParse(value[0], out var itemId) && long.TryParse(value[1], out providerId);

            if (!isSucces)
            {
                continue;
            }

            result.Add((new(itemId), new(providerId)));
        }

        return result;
    }

    private User? GetCurrentUser()
    {
        if (User.Identity is null || User.Identity.Name is null)
            return null;

        return _usersRepository.GetByEmail(User.Identity.Name);
    }
}
