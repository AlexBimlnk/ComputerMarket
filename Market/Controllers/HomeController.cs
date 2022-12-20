using System.Diagnostics;

using Market.Logic.Storage;
using Market.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MarketContext _context;

    public HomeController(ILogger<HomeController> logger, MarketContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult Privacy() => View();

    [Authorize(Policy = "OnlyForAgents")]
    public IActionResult Provider() => View();

    [Authorize(Policy = "OnlyForManager")]
    public IActionResult Manager() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
