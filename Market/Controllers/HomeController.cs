using System.Diagnostics;

using Market.Logic.Models;
using Market.Logic.Models.Abstractions;
using Market.Logic.Storage;
using Market.Logic.Storage.Repositories;
using Market.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MarketContext _context;
    private readonly IUsersRepository _usersRepository;
    private readonly ICatalog _catalog;

    public HomeController(
        ICatalog catalog, 
        IUsersRepository usersRepository, 
        ILogger<HomeController> logger, 
        MarketContext context)
    {
        _context = context;
        _logger = logger;
        _catalog = catalog;
        _usersRepository = usersRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var user = GetCurrentUser();

        var viewModel = new HomeIndexViewModel();

        if (user is not null)
        {
            viewModel.LoginUserType = user.Type;
        }

        viewModel.Types = _catalog.GetItemTypes().Take(4);

        return View(viewModel);
    }

    [HttpGet("home/api/index")]
    public HomeIndexViewModel ApiIndex()
    {
        var user = GetCurrentUser();

        var viewModel = new HomeIndexViewModel();

        if (user is not null)
        {
            viewModel.LoginUserType = user.Type;
        }

        viewModel.Types = _catalog.GetItemTypes().Take(4);

        return viewModel;
    }

    [HttpGet]
    public IActionResult Privacy() => View();

    [Authorize(Policy = "OnlyForAgents")]
    public IActionResult Provider() => View();

    [Authorize(Policy = "OnlyForManager")]
    public IActionResult Manager() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    private User? GetCurrentUser()
    {
        if (User.Identity is null || User.Identity.Name is null)
            return null;

        return _usersRepository.GetByEmail(User.Identity.Name);
    }
}
