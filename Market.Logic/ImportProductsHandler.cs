using General.Logic;
using General.Storage;
using General.Transport;

using Market.Logic.Markers;
using Market.Logic.Models;
using Market.Logic.Storage.Repositories;
using Market.Logic.Transport.Models;

using Microsoft.Extensions.Logging;

namespace Market.Logic;

/// <summary xml:lang = "ru">
/// Обработчки приема новых продуктов от сервиса импорта.
/// </summary>
public sealed class ImportProductsHandler : IAPIRequestHandler<ImportMarker>
{
    private readonly ILogger<ImportProductsHandler> _logger;
    private readonly IDeserializer<string, IReadOnlyCollection<TransportProduct>> _deserializer;
    private readonly IProductsRepository _productRepository;
    private readonly IItemsRepository _itemRepository;
    private readonly IKeyableRepository<Provider, ID> _providerRepository;

    /// <summary xml:lang = "ru">
    /// Создаёт экземлпяр класса <see cref="ImportProductsHandler"/>.
    /// </summary>
    /// <param name="deserializer" xml:lang = "ru">Десериализатор продуктов.</param>
    /// <param name="itemRepository" xml:lang = "ru">Репозиторий товаров.</param>
    /// <param name="providerRepository" xml:lang = "ru">Репозиторий провайдеров.</param>
    /// <param name="productRepository" xml:lang = "ru">Репозиторий продуктов.</param>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    /// <exception cref="ArgumentNullException">
    /// Если один из параметров - <see langword="null"/>.
    /// </exception>
    public ImportProductsHandler(
        IDeserializer<string, IReadOnlyCollection<TransportProduct>> deserializer,
        IItemsRepository itemRepository,
        IKeyableRepository<Provider, ID> providerRepository,
        IProductsRepository productRepository,
        ILogger<ImportProductsHandler> logger)
    {
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
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
        {
            var domainProduct = _productRepository.GetByKey((
                new(product.InternalID),
                new(product.ProviderID)));

            Product addOrUpdateProduct;

            if (domainProduct is null)
            {
                var item = _itemRepository.GetByKey(new(product.InternalID))!;
                var provider = _providerRepository.GetByKey(new(product.ProviderID))!;

                addOrUpdateProduct = new Product(
                    item,
                    provider,
                    new Price(product.Price),
                    product.Quantity);
            }
            else
            {
                addOrUpdateProduct = new Product(
                    domainProduct.Item,
                    domainProduct.Provider,
                    new Price(product.Price),
                    product.Quantity);
            }

            await _productRepository.AddOrUpdateAsync(addOrUpdateProduct);
        }

        _productRepository.Save();

        _logger.LogInformation("Request processing complete");
    }
}
