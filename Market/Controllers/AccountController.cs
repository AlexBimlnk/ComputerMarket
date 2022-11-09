using System.Security.Claims;

using General.Storage;

using Market.Logic;
using Market.Logic.Models;
using Market.Logic.Storage;
using Market.Models.Account;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Market.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IKeyableRepository<User, string> _usersRepository;

    public AccountController(IKeyableRepository<User, string> usersRepository, ILogger<HomeController> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register() => View();


    [HttpGet]
    public IActionResult Login() => View();


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User? user = _usersRepository.GetByKey(model.Email);
            if (user is not null)
            {
                await AuthenticateAsync(model.Email);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAsync(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            User? user = _usersRepository.GetByKey(model.Email);
            if (user is null)
            {

                await _usersRepository.AddAsync(new User(new InternalID(-1), model.Login, new Password(model.Password), model.Email, UserType.Customer));
                _usersRepository.Save();

                await AuthenticateAsync(model.Email);

                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }
        return View(model);
    }

    private async Task AuthenticateAsync(string userName)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
            };

        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
