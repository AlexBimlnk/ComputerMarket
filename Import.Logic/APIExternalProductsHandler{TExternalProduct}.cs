using Import.Logic.Abstractions;
using Import.Logic.Models;

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
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из аргументов оказался <see langword="null"/>.
    /// </exception>
    public APIExternalProductsHandler(
        ILogger logger,
        IAPIProductFetcher<TExternalProduct> fetcher,
        IMapper<Product> mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fetcher = fetcher ?? throw new ArgumentNullException(nameof(fetcher));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task HandleAsync(string request)
    {
        _logger.LogInformation("Handle request..");

        var products = await _fetcher.FetchProductsAsync(request);

        var mappedProducts = _mapper.MapCollection(products);

        // сендер
    }
}
