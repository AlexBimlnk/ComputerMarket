using General.Logic.Executables;
using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Models;
using Market.Logic.Queries;
using Market.Logic.Queries.Import;
using Market.Logic.Storage.Repositories;
using Market.Logic.Transport.Configurations;
using Market.Logic.Transport.Senders;
using Market.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

using IQuerySender = IQuerySender<ImportCommandConfigurationSender, ImportQuery, QueryResult<IReadOnlyCollection<Link>>>;

/// <summary xml:lang = "ru">
/// Контроллер для управления связями между внутренними и внешними продуктами поставщиков.
/// </summary>
[Authorize(Policy = "OnlyForManager")]
public sealed class LinksController : Controller
{
    private readonly ILogger<LinksController> _logger;
    private readonly IProvidersRepository _providerRepository;
    private readonly IQuerySender _querySender;
    private readonly ISender<ImportCommandConfigurationSender, ImportCommand> _importCommandSender;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="LinksController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="providerRepository" xml:lang = "ru">
    /// Репозиторий провайдеров.
    /// </param>
    /// <param name="querySender" xml:lang = "ru">
    /// Отправитель запросов.
    /// </param>
    /// <param name="importCommandSender" xml:lang = "ru">
    /// Отправитель команд.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public LinksController(
        ILogger<LinksController> logger,
        IProvidersRepository providerRepository,
        IQuerySender querySender,
        ISender<ImportCommandConfigurationSender, ImportCommand> importCommandSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));
        _querySender = querySender ?? throw new ArgumentNullException(nameof(querySender));
        _importCommandSender = importCommandSender ?? throw new ArgumentNullException(nameof(importCommandSender));
    }

    // Get: Links/List
    /// <summary xml:lang = "ru">
    /// Возвращает список связей.
    /// </summary>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var result = await _querySender.SendAsync(
            new GetLinksQuery(new ExecutableID(Guid.NewGuid().ToString())));

        var links = result.Result;

        return View(links);
    }

    // GET: Links/Create
    /// <summary xml:lang = "ru">
    /// Возвращает форму для создания связей.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    [HttpGet]
    public ActionResult Create() => View();

    // POST: Links/Create
    /// <summary xml:lang = "ru">
    /// Запрос на создание новой связи.
    /// </summary>
    /// <param name="link" xml:lang = "ru"> 
    /// Связь которую необходимо добавить в систему. 
    /// </param>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateAsync(LinkViewModel link)
    {
        if (ModelState.IsValid)
        {
            var provider = _providerRepository.GetByKey(new(link.ProviderId))!;

            await _importCommandSender.SendAsync(new SetLinkCommand(
                new(Guid.NewGuid().ToString()),
                new(link.InternalId),
                new(link.ExternalId),
                provider));

            return RedirectToAction("List");
        }

        return View();
    }

    // GET: Links/Delete
    /// <summary xml:lang = "ru">
    /// Возвращает форму для удаления связей.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    [HttpGet]
    public ActionResult Delete(LinkViewModel link)
    {
        if (ModelState.IsValid)
        {
            return View(link);
        }

        return RedirectToAction("List");
    }

    // POST: Links/Delete
    /// <summary xml:lang = "ru">
    /// Запрос на удаление связи.
    /// </summary>
    /// <param name="link" xml:lang = "ru"> 
    /// Связь которую необходимо удалить из системы. 
    /// </param>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteAsync(LinkViewModel link)
    {
        if (ModelState.IsValid)
        {
            var provider = _providerRepository.GetByKey(new(link.ProviderId))!;

            await _importCommandSender.SendAsync(new DeleteLinkCommand(
                new(Guid.NewGuid().ToString()),
                new(link.ExternalId),
                provider));

            return RedirectToAction("List");
        }

        return View();
    }
}
