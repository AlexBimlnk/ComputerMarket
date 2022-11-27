using General.Logic.Executables;
using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Models;
using Market.Logic.Queries;
using Market.Logic.Queries.Import;
using Market.Logic.Transport.Configurations;
using Market.Logic.Transport.Senders;
using Market.Models;

using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

using IQuerySender = IQuerySender<ImportCommandConfigurationSender, ImportQuery, QueryResult<IReadOnlyCollection<Link>>>;

public class LinksController : Controller
{
    private readonly ILogger<LinksController> _logger;
    private readonly IQuerySender _querySender;
    private readonly ISender<ImportCommandConfigurationSender, ImportCommand> _importCommandSender;

    public LinksController(
        ILogger<LinksController> logger,
        IQuerySender querySender,
        ISender<ImportCommandConfigurationSender, ImportCommand> importCommandSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _querySender = querySender ?? throw new ArgumentNullException(nameof(querySender));
        _importCommandSender = importCommandSender ?? throw new ArgumentNullException(nameof(importCommandSender));
    }


    public async Task<IActionResult> ListAsync()
    {
        var result = await _querySender.SendAsync(
            new GetLinksQuery(new ExecutableID(Guid.NewGuid().ToString())));

        var links = result.Result;

        return View(links);
    }

    // GET: LinksController/Create
    public ActionResult Create() => View();

    // POST: LinksController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateAsync(LinkViewModel link)
    {
        if (ModelState.IsValid)
        {
            // ToDo: get provider
            //var provider = _repo.GetProviderById

            await _importCommandSender.SendAsync(new SetLinkCommand(
                new(Guid.NewGuid().ToString()),
                new(link.InternalId),
                new(link.ExternalId),
                new(
                    new(link.ProviderId),
                    "some name",
                    new(1.5m),
                    new PaymentTransactionsInformation("0123456789", "01234012340123401234"))));

            return RedirectToAction("List");
        }

        return View();
    }
}
