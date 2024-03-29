﻿using Market.Logic;
using Market.Logic.Reports;
using Market.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер для создания отчетов.
/// </summary>
[Authorize]
public sealed class ReportController : Controller
{
    private readonly ILogger<ReportController> _logger;
    private readonly IReportBuilder _reportBuilder;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="ReportController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="reportBuilder" xml:lang = "ru">
    /// Создатель отчетов.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public ReportController(
        ILogger<ReportController> logger,
        IReportBuilder reportBuilder)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _reportBuilder = reportBuilder ?? throw new ArgumentNullException(nameof(reportBuilder));
    }

    // GET: report/details
    /// <summary xml:lang = "ru">
    /// Возвращает форму для представления отчета.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    [Authorize(Policy = "OnlyForManager")]
    [HttpGet]
    public ActionResult Details(Report report) => View(report);

    // GET: report/create
    /// <summary xml:lang = "ru">
    /// Возвращает форму для создания отчетов.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    [Authorize(Policy = "OnlyForManager")]
    [HttpGet]
    public ActionResult Create() => View();

    // POST: report/create
    /// <summary xml:lang = "ru">
    /// Запрос на создание новой связи.
    /// </summary>
    /// <param name="link" xml:lang = "ru"> 
    /// Связь которую необходимо добавить в систему. 
    /// </param>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "OnlyForManager")]
    public ActionResult Create(ReportViewModel reportViewModel)
    {
        if (ModelState.IsValid)
        {
            _reportBuilder.SetProviderId(new ID(reportViewModel.ProviderId));
            _reportBuilder.SetStartPeriod(DateOnly.FromDateTime(reportViewModel.StartPeriod));
            _reportBuilder.SetEndPeriod(DateOnly.FromDateTime(reportViewModel.EndPeriod));

            try
            {
                var report = _reportBuilder.CreateReport();
                return View("Details", report);
            }
            catch (InvalidOperationException)
            {
                ViewBag.ErrorMessage = "За данный период невозможно построить отчет.";
                return View("Details");
            }            
        }

        return View();
    }

    [HttpPost("report/api/create")]
    [Authorize(Policy = "OnlyForManager")]
    public Report ApiCreate([FromBody] ReportViewModel reportViewModel)
    {
        _reportBuilder.SetProviderId(new ID(reportViewModel.ProviderId));
        _reportBuilder.SetStartPeriod(DateOnly.FromDateTime(reportViewModel.StartPeriod));
        _reportBuilder.SetEndPeriod(DateOnly.FromDateTime(reportViewModel.EndPeriod));

        return _reportBuilder.CreateReport();
    }
}
