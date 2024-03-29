﻿using Market.Logic.ComputerBuilder;
using Market.Logic.Models;
using Market.Logic.Models.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер для сборки компьютера.
/// </summary>
[AllowAnonymous]
public class BuilderController : Controller
{
    private const int PROCESSOR_TYPE_ID = 1;
    private const int MOTHER_TYPE_ID = 4;

    private readonly ILogger<BuilderController> _logger;
    private readonly IComputerBuilder _computerBuilder;
    private readonly ICatalog _catalog;

    /// <summary xml:lang = "ru">
    /// Создаёт экземпляр класса <see cref="BuilderController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    /// <param name="computerBuilder" xml:lang = "ru">
    /// Сборщик компьютеров.
    /// </param>
    /// <param name="catalog" xml:lang = "ru">
    /// Каталог.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если один из параметров - <see langword="null"/>.
    /// </exception>
    public BuilderController(
        ILogger<BuilderController> logger,
        IComputerBuilder computerBuilder,
        ICatalog catalog)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _computerBuilder = computerBuilder ?? throw new ArgumentNullException(nameof(computerBuilder));
        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    private void PrepareView()
    {
        ViewBag.Processors = _catalog.GetItems().Where(x => x.Type.Id == PROCESSOR_TYPE_ID)
            .Select(x => x.Name);
        ViewBag.MotherBoards = _catalog.GetItems().Where(x => x.Type.Id == MOTHER_TYPE_ID)
            .Select(x => x.Name);
    }

    private void CreateBestOffer(
        IEnumerable<Product> processors,
        IEnumerable<Product> motherBoards)
    {
        var bestOffer = new List<Product>()
        {
            processors.MinBy(x => x.FinalCost)!,
            motherBoards.MinBy(x => x.FinalCost)!,
        };

        ViewBag.BestOffer = bestOffer;
    }

    /// <summary xml:lang = "ru">
    /// Возвращает форму билдера.
    /// </summary>
    /// <returns> <see cref="IActionResult"/>. </returns>
    [HttpGet]
    public IActionResult Index()
    {
        PrepareView();

        return View();
    }

    /// <summary xml:lang = "ru">
    /// Пытается собрать сборку.
    /// </summary>
    /// <returns> <see cref="IActionResult"/>. </returns>
    [HttpPost]
    public IActionResult Build()
    {
        PrepareView();

        var cpuName = Request.Form["processor"].ToString();
        var motherBoardName = Request.Form["motherBoard"].ToString();

        var possibleProcessors = _catalog.GetProducts(new CatalogFilter())
            .Where(x => x.Item.Type.Id == PROCESSOR_TYPE_ID && x.Item.Name == cpuName);

        var possibleMotherBoards = _catalog.GetProducts(new CatalogFilter())
            .Where(x => x.Item.Type.Id == MOTHER_TYPE_ID && x.Item.Name == motherBoardName);

        _computerBuilder.AddOrReplace(possibleProcessors.First()!.Item);
        _computerBuilder.AddOrReplace(possibleMotherBoards.First()!.Item);

        var result = _computerBuilder.Build();

        if (result.IsSucces)
            CreateBestOffer(possibleProcessors, possibleMotherBoards);

        return View("Index", result);
    }

    [HttpPost("builder/api/build")]
    public IComputerBuildResult ApiBuild()
    {
        PrepareView();

        var cpuName = Request.Form["processor"].ToString();
        var motherBoardName = Request.Form["motherBoard"].ToString();

        var possibleProcessors = _catalog.GetProducts(new CatalogFilter())
            .Where(x => x.Item.Type.Id == PROCESSOR_TYPE_ID && x.Item.Name == cpuName);

        var possibleMotherBoards = _catalog.GetProducts(new CatalogFilter())
            .Where(x => x.Item.Type.Id == MOTHER_TYPE_ID && x.Item.Name == motherBoardName);

        _computerBuilder.AddOrReplace(possibleProcessors.First()!.Item);
        _computerBuilder.AddOrReplace(possibleMotherBoards.First()!.Item);

        var result = _computerBuilder.Build();

        if (result.IsSucces)
            CreateBestOffer(possibleProcessors, possibleMotherBoards);

        return result;
    }
}
