using General.Logic.Executables;
using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Models;
using Market.Logic.Queries.Import;
using Market.Logic.Transport.Configurations;
using Market.Logic.Transport.Senders;

using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

using IQuerySender = IQuerySender<ImportCommandConfigurationSender, GetLinksQuery, IReadOnlyCollection<Link>>;

public class LinksController : Controller
{
    private readonly ILogger<LinksController> _logger;
    private readonly IQuerySender _querySender;
    private readonly ISender<ImportCommandConfigurationSender, SetLinkCommand> _setLinkSender;
    private readonly ISender<ImportCommandConfigurationSender, DeleteLinkCommand> _deleteLinkSender;

    public LinksController(
        ILogger<LinksController> logger,
        IQuerySender querySender,
        ISender<ImportCommandConfigurationSender, SetLinkCommand> setLinkSender,
        ISender<ImportCommandConfigurationSender, DeleteLinkCommand> deleteLinkSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _querySender = querySender ?? throw new ArgumentNullException(nameof(querySender));
        _setLinkSender = setLinkSender ?? throw new ArgumentNullException(nameof(setLinkSender));
        _deleteLinkSender = deleteLinkSender ?? throw new ArgumentNullException(nameof(deleteLinkSender));
    }


    public async Task<IActionResult> LinksListAsync()
    {
        var links = await _querySender.SendAsync(
            new GetLinksQuery(new ExecutableID(Guid.NewGuid().ToString())));

        return View(links);
    }
}
