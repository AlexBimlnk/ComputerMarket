using General.Storage;

using Market.Logic;
using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[Authorize]
public class ProviderController : Controller
{
    private readonly IKeyableRepository<Provider, ID> _providerRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<ProviderController> _logger;
    public ProviderController(IKeyableRepository<Provider, ID> providerRepository,
        IUsersRepository usersRepository,
        ILogger<ProviderController> logger)
    {
        _providerRepository = providerRepository;
        _usersRepository = usersRepository;
        _logger = logger;
    }

    [HttpGet("/newprovider")]
    public IActionResult Create() => View();

    [HttpPost("/newprovider")]
    public IActionResult Create()
    {

    }

    [HttpGet]
    public IActionResult Providers() => View();

    [HttpGet]
    public IActionResult Provider() => View();
}
