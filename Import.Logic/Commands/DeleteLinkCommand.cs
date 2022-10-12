using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Abstractions;
using Import.Logic.Abstractions.Commands;
using Import.Logic.Models;

namespace Import.Logic.Commands;
public class DeleteLinkCommand : CommandBase
{
    private readonly DeleteLinkCommandParameters _parameters;
    private readonly ICache<Link> _cacheLinks;
    private readonly IRepository<Link> _linkRepository;

    public DeleteLinkCommand(
        DeleteLinkCommandParameters parameters, 
        ICache<Link> cacheLinks, 
        IRepository<Link> linkRepository)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _cacheLinks = cacheLinks ?? throw new ArgumentNullException(nameof(cacheLinks));
        _linkRepository = linkRepository ?? throw new ArgumentNullException(nameof(linkRepository));
    }

    public override CommandID Id => _parameters.Id;

    protected override Task ExecuteCoreAsync()
    {
        var link = new Link(_parameters.InternalID, _parameters.ExternalID);

        if (!_cacheLinks.Contains(link))
            throw new InvalidOperationException("Such a link doesn't exist.");

        _linkRepository.Delete(link);

        _linkRepository.Save();

        _cacheLinks.Delete(link);

        return new Task(() => _linkRepository.Delete(link));
    }
}
