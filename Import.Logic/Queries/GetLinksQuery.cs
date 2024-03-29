﻿using General.Logic.Executables;
using General.Logic.Queries;
using General.Storage;
using General.Transport;

using Import.Logic.Abstractions;
using Import.Logic.Models;

namespace Import.Logic.Queries;

/// <summary xml:lang = "ru">
/// Запрос на получение связей.
/// </summary>
public sealed class GetLinksQuery : QueryBase<IReadOnlyCollection<Link>>
{
    private readonly GetLinksQueryParameters _parameters;
    private readonly IRepository<Link> _repository;
    
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="GetLinksQuery"/>.
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры запроса.
    /// </param>
    /// <param name="repository" xml:lang = "ru">
    /// Репозиторий связей.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из параметров равен <see langword="null"/>.
    /// </exception>
    public GetLinksQuery(
        GetLinksQueryParameters parameters,
        IRepository<Link> repository)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc/>
    public override ExecutableID Id => _parameters.Id;

    protected override Task ExecuteCoreAsync()
    {
        Result = _repository.GetEntities().ToList();
        return Task.CompletedTask;
    }
}