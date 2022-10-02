using Import.Logic.Abstractions;
using Import.Logic.Abstractions.Commands;
using Import.Logic.Models;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда на установку связи.
/// </summary>
public sealed class SetLinkCommand : CommandBase
{
    private readonly SetLinkCommandParameters _parameters;
    private readonly ICache<Link> _cacheLinks;
    private readonly IRepository<Link> _linkRepository;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="SetLinkCommand"/>.
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры команды.
    /// </param>
    /// <param name="cacheLinks" xml:lang = "ru">
    /// Кэш связей.
    /// </param>
    /// <param name="linkRepository" xml:lang = "ru">
    /// Репозиторий связей.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из параметров равен <see langword="null"/>.
    /// </exception>
    public SetLinkCommand(
        SetLinkCommandParameters parameters,
        ICache<Link> cacheLinks,
        IRepository<Link> linkRepository)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _cacheLinks = cacheLinks ?? throw new ArgumentNullException(nameof(cacheLinks));
        _linkRepository = linkRepository ?? throw new ArgumentNullException(nameof(linkRepository));
    }

    /// <inheritdoc/>
    public override CommandID Id => _parameters.Id;

    protected override async Task ExecuteCoreAsync()
    {
        var link = new Link(_parameters.InternalID, _parameters.ExternalID);

        if (_cacheLinks.Contains(link))
            throw new InvalidOperationException("Such a link already exists.");

        await _linkRepository.AddAsync(link);
        
        _linkRepository.Save();

        _cacheLinks.Add(link);
    }
}