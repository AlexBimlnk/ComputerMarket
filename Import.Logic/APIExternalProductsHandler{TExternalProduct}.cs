using Import.Logic.Abstractions;
using Import.Logic.Models;
using Import.Logic.Transport.Configuration;

using Microsoft.Extensions.Logging;

namespace Import.Logic;

/// <summary xml:lang = "ru">
/// Обработчик пришедших внешних продуктов по API.
/// </summary>
/// <typeparam name="TExternalProduct" xml:lang = "ru">
/// Маркерный тип внешнего обрабатываемого продукта.
/// </typeparam>
public class APIExternalProductsHandler<TExternalProduct> : IAPIExternalProductHandler<TExternalProduct>
{
    private readonly ILogger _logger;
    private readonly IAPIProductFetcher<TExternalProduct> _fetcher;
    private readonly IMapper<Product> _mapper;
    private readonly ISender<InternalProductSenderConfiguration, IReadOnlyCollection<Product>> _productsSender;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="APIExternalProductsHandler{TExternalProduct}"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="fetcher" xml:lang = "ru">
    /// Получатель продуктов.
    /// </param>
    /// <param name="mapper" xml:lang = "ru">
    /// Маппер.
    /// </param>
    /// <param name="productsSender" xml:lang = "ru">
    /// Отправитель смапленных продуктов.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из аргументов оказался <see langword="null"/>.
    /// </exception>
    public APIExternalProductsHandler(
        ILogger logger,
        IAPIProductFetcher<TExternalProduct> fetcher,
        IMapper<Product> mapper,
        ISender<InternalProductSenderConfiguration, IReadOnlyCollection<Product>> productsSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fetcher = fetcher ?? throw new ArgumentNullException(nameof(fetcher));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productsSender = productsSender ?? throw new ArgumentNullException(nameof(productsSender));
    }

    /// <inheritdoc/>
    public async Task HandleAsync(string request, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(request))
            throw new ArgumentException(
                "The request can't be null, empty or has only whitespaces.",
                nameof(request));

        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Handle request..");

        var products = await _fetcher.FetchProductsAsync(request, token);

        var mappedProducts = _mapper.MapCollection(products);

        await _productsSender.SendAsync(mappedProducts, token);

        _logger.LogInformation("The request be handled");
    }
}
