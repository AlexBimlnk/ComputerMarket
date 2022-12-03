using General.Logic;
using General.Storage;
using General.Transport;

using Market.Logic.Markers;
using Market.Logic.Models;
using Market.Logic.Models.WT;

using Microsoft.Extensions.Logging;

namespace Market.Logic;

/// <summary xml:lang = "ru">
/// Обработчки информации о транзакциях от сервиса WT.
/// </summary>
public sealed class TransactionRequestHandler : IAPIRequestHandler<WTMarker>
{
    private readonly ILogger<TransactionRequestHandler> _logger;
    private readonly IDeserializer<string, TransactionRequestResult> _deserializer;
    private readonly IRepository<Order> _repositoryProduct;

    /// <summary xml:lang = "ru">
    /// Создаёт экземлпяр класса <see cref="TransactionRequestHandler"/>.
    /// </summary>
    /// <param name="deserializer" xml:lang = "ru">Десериализатор продуктов.</param>
    /// <param name="orderRepository" xml:lang = "ru">Репозиторий продуктов.</param>
    /// <param name="logger" xml:lang = "ru">Логгер.</param>
    /// <exception cref="ArgumentNullException">
    /// Если один из параметров - <see langword="null"/>.
    /// </exception>
    public TransactionRequestHandler(
        IDeserializer<string, TransactionRequestResult> deserializer,
        IRepository<Order> orderRepository,
        ILogger<TransactionRequestHandler> logger)
    {
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        _repositoryProduct = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
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

        //foreach (var product in products)
        //    await _repositoryProduct.AddAsync(product);

        //_repositoryProduct.Save();

        _logger.LogInformation("Request processing complete");
    }
}
