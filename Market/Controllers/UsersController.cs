using Market.Logic.Storage;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Market.Controllers;

public class UsersController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MarketContext _context;

    public UsersController(ILogger<HomeController> logger, MarketContext context)
    {
        _context = context;
        _logger = logger;
    }

    // GET: UsersController
    [HttpGet]
    public async Task<IActionResult> IndexAsync() => View(await _context.Users.ToListAsync());

    [HttpGet]
    public IActionResult Info() => View();
}
