using General.Logic;
using General.Storage;
using General.Transport;

using Market.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Market.Logic;

/// <summary xml:lang = "ru">
/// Обработчки приема новых продуктов от сервиса импорта.
/// </summary>
public sealed class ImportProductsHandler : IAPIRequestHandler<IReadOnlyCollection<Product>>
{
    private readonly ILogger<ImportProductsHandler> _logger;
    private readonly IDeserializer<string, IReadOnlyCollection<Product>> _deserializer;
    private readonly IRepository<Product> _repositoryProduct;

    /// <summary xml:lang = "ru">
    /// Создаёт экземлпяр класса <see cref="ImportProductsHandler"/>.
    /// </summary>
    /// <param name="deserializer" xml:lang = "ru">Десериализатор продуктов.</param>
    /// <param name="repositoryProduct" xml:lang = "ru">Репозиторий продуктов.</param>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    /// <exception cref="ArgumentNullException">
    /// Если один из параметров - <see langword="null"/>.
    /// </exception>
    public ImportProductsHandler(
        IDeserializer<string, IReadOnlyCollection<Product>> deserializer,
        IRepository<Product> repositoryProduct,
        ILogger<ImportProductsHandler> logger)
    {
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        _repositoryProduct = repositoryProduct ?? throw new ArgumentNullException(nameof(repositoryProduct));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task HandleAsync(string request, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(request))
            throw new ArgumentException("Source can't be null or empty or contains only whitespaces");

        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Processing new request");

        var products = _deserializer.Deserialize(request);

        _logger.LogDebug("Deserialize {Sourse} to Products comlete", request);

        foreach (var product in products)
            await _repositoryProduct.AddAsync(product);

        _repositoryProduct.Save();

        _logger.LogInformation("Request processing complete");
    }
}
