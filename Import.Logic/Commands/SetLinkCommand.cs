using Import.Logic.Abstractions;
using Import.Logic.Abstractions.Commands;
using Import.Logic.Models;

using Microsoft.Extensions.DependencyInjection;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда на установку связи.
/// </summary>
public sealed class SetLinkCommand : CommandBase
{
    private readonly SetLinkCommandParameters _parameters;
    private readonly ICache<Link> _cacheLinks;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="SetLinkCommand"/>.
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
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из параметров равен <see langword="null"/>.
    /// </exception>
    public SetLinkCommand(
        SetLinkCommandParameters parameters,
        ICache<Link> cacheLinks,
        IServiceScopeFactory scopeFactory)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _cacheLinks = cacheLinks ?? throw new ArgumentNullException(nameof(cacheLinks));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    /// <inheritdoc/>
    public override CommandID Id => _parameters.Id;

    protected override async Task ExecuteCoreAsync()
    {
        var link = new Link(_parameters.InternalID, _parameters.ExternalID);

        if (_cacheLinks.Contains(link))
            throw new InvalidOperationException("Such a link already exists.");

        using var scope = _scopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Link>>();

        await repository.AddAsync(link)
            .ConfigureAwait(false);

        repository.Save();

        _cacheLinks.Add(link);
    }
}