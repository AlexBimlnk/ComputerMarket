using General.Logic.Executables;
using General.Storage;

using Import.Logic.Abstractions;
using Import.Logic.Models;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда на удаление связи.
/// </summary>
public sealed class DeleteLinkCommand : CommandBase
{
    private readonly DeleteLinkCommandParameters _parameters;
    private readonly IKeyableCache<Link, ExternalID> _cacheLinks;
    private readonly IRepository<Link> _repository;

    /// <summary>
    /// Создает новый экземпляр типа <see cref="DeleteLinkCommand"/>.
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры команды.
    /// </param>
    /// <param name="cacheLinks" xml:lang = "ru">
    /// Кэш связей.
    /// </param>
    /// <param name="scopeFactory" xml:lang = "ru">
    /// Фабрика сервисов с областью применения.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Когда любой из параметров равен <see langword="null"/>.
    /// </exception>
    public DeleteLinkCommand(
        DeleteLinkCommandParameters parameters,
        IKeyableCache<Link, ExternalID> cacheLinks,
        IRepository<Link> repository)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _cacheLinks = cacheLinks ?? throw new ArgumentNullException(nameof(cacheLinks));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc/>
    public override ExecutableID Id => _parameters.Id;

    /// <inheritdoc/>
    protected override Task ExecuteCoreAsync()
    {
        var link = _cacheLinks.GetByKey(_parameters.ExternalID);

        if (link is null)
            throw new InvalidOperationException("Such a link doesn't exist.");

        _repository.Delete(link);

        _repository.Save();

        _cacheLinks.Delete(link);

        return Task.CompletedTask;
    }
}
