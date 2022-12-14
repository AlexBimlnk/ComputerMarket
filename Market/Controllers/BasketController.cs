using Market.Logic.Storage.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;
public class BasketController : Controller
{
    private readonly IUsersRepository _usersRepository;
    private readonly IBasketRepository _basketRepository;

    public BasketController(IBasketRepository basketRepository, IUsersRepository usersRepository)
    {
        _basketRepository = basketRepository;
        _usersRepository = usersRepository;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        if (User.Identity is null || User.Identity.Name is null)
            return View();

        var user = _usersRepository.GetByEmail(User.Identity.Name);

        if (user is null)
            return View();

        var items = _basketRepository.GetAllBasketItems(user);

        return View(items);
    }

    [HttpDelete]
    [Authorize]
    public IActionResult Remove()
    {
        var a = 2;
        return null;
    }

    [HttpPut]
    [Authorize]
    public IActionResult DecreaseProducts()
    {
        var a = 2;
        return null;
    }

    [HttpPost]
    public IActionResult CreateOrder()
    {
        var a = 2;
        return null;
    }
}
