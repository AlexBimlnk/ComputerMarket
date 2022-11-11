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
using Market.Logic.Storage.Repositories;

namespace Market.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUsersRepository _usersRepository;

    public AccountController(IUsersRepository usersRepository, ILogger<HomeController> logger)
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
            if (_usersRepository.IsCanAuthenticate(model.Email, model.Password, out var user))
            {
                await AuthenticateAsync(user);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Некорректные логин и(или) пароль");
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User? user = _usersRepository.GetByEmail(model.Email);
            if (user is null)
            {
                user = new User(default, new AuthenticationData(model.Login, model.Email, new Password(model.Password)), UserType.Customer);
                await _usersRepository.AddAsync(user);
                _usersRepository.Save();

                await AuthenticateAsync(user);

                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError(string.Empty, "Некорректные логин и(или) пароль");
        }
        return View(model);
    }

    private async Task AuthenticateAsync(User user)
    {
        var role = user.Type.ToString();

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.AuthenticationData.Email),
            new Claim("role", role)
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
