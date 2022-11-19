using General.Logic.Commands;
using General.Storage;

using Import.Logic.Abstractions;
using Import.Logic.Models;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда на получение связей.
/// </summary>
public sealed class GetLinksCommand : CallbackCommandBase<IReadOnlyCollection<Link>>
{
    private readonly GetLinksCommandParameters _parameters;
    private readonly IRepository<Link> _repository;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="GetLinksCommand"/>.
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры команды.
    /// </param>
    /// <param name="repository" xml:lang = "ru">
    /// Репозиторий связей.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из параметров равен <see langword="null"/>.
    /// </exception>
    public GetLinksCommand(
        GetLinksCommandParameters parameters,
        IRepository<Link> repository)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc/>
    public override CommandID Id => _parameters.Id;

    protected override Task ExecuteCoreAsync()
    {
        Result = _repository.GetEntities().ToArray();
        return Task.CompletedTask;  
    }
}