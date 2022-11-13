using System.Security.Claims;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;
using Market.Models.Account;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер для управления учётными записями пользователей.
/// </summary>
public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUsersRepository _usersRepository;

    /// <summary xml:lang = "ru">
    /// Создаёт экземпляр класса <see cref="AccountController"/>.
    /// </summary>
    /// <param name="usersRepository" xml:lang = "ru">Репозиторий пользователей.</param>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если один из параметров - <see langword="null"/>.
    /// </exception>
    public AccountController(IUsersRepository usersRepository, ILogger<HomeController> logger)
    {
        _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary xml:lang = "ru">
    /// Запрос возврашающий форму для регитсрации.
    /// </summary>
    /// <returns xml:lang = "ru">Представление с формой для регистрации.</returns>
    [HttpGet]
    public IActionResult Register() => View();

    /// <summary xml:lang = "ru">
    /// Запрос вовзращающий форму для входа.
    /// </summary>
    /// <returns xml:lang = "ru">Представление с формой для входа в систему.</returns>
    [HttpGet]
    public IActionResult Login() => View();

    /// <summary xml:lang = "ru">
    /// Запрос на вход в систему.
    /// </summary>
    /// <param name="model">Модель с данными для входа пользователя в систему.</param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (_usersRepository.IsCanAuthenticate(new AuthenticationData(model.Email, new Password(model.Password)), out var user))
            {
                await AuthenticateAsync(user);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Некорректные логин и(или) пароль");
        }
        return View(model);
    }

    /// <summary xml:lang = "ru">
    /// Запрос на регистрацию в системе.
    /// </summary>
    /// <param name="model">Модель с данными для регистрации пользователя в системе.</param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var data = new AuthenticationData(model.Email, new Password(model.Password), model.Login);
            User user;
            if (!_usersRepository.IsCanAuthenticate(data, out user))
            {
                user = new User(default, data, UserType.Customer);

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

    /// <summary xml:lang = "ru">
    /// Запрос на выход из системы.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
