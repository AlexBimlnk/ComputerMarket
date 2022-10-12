using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Import.Logic.Transport.Receivers;

/// <summary xml:lang = "ru">
/// Получатель внешних продуктов по API.
/// </summary>
/// <typeparam name="TExternalProductModel" xml:lang = "ru">
/// Тип внешних получаемых продуктов.
/// </typeparam>
public sealed class APIProductFetcher<TExternalProductModel> : IAPIProductFetcher<TExternalProductModel>
{
    private readonly ILogger<APIProductFetcher<TExternalProductModel>> _logger;
    private readonly IDeserializer<string, IReadOnlyCollection<TExternalProductModel>> _deserializer;
    private readonly IHistoryRecorder _historyRecorder;
    private readonly IConverter<TExternalProductModel, Product> _converter;
    private readonly IConverter<TExternalProductModel, History> _historyConverter;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="APIProductFetcher{TExternalProductModel}"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="deserializer" xml:lang = "ru">
    /// Десериализатор запросов.
    /// </param>
    /// <param name="historyRecorder" xml:lang = "ru">
    /// Записыватель истории о получении продуктов.
    /// </param>
    /// <param name="converter" xml:lang = "ru">
    /// Конвертер из модели внешнего продукта во внутреннюю.
    /// </param>
    /// <param name="historyConverter" xml:lang = "ru">
    /// Конвертер внешних моделей продуктов в историю.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда любой из параметров оказался <see langword="null"/>.
    /// </exception>
    public APIProductFetcher(
        ILogger<APIProductFetcher<TExternalProductModel>> logger,
        IDeserializer<string, IReadOnlyCollection<TExternalProductModel>> deserializer,
        IHistoryRecorder historyRecorder,
        IConverter<TExternalProductModel, Product> converter,
        IConverter<TExternalProductModel, History> historyConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        _historyRecorder = historyRecorder ?? throw new ArgumentNullException(nameof(historyRecorder));
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        _historyConverter = historyConverter ?? throw new ArgumentNullException(nameof(converter));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<Product>> FetchProductsAsync(string request, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(request))
            throw new ArgumentException(nameof(request));

        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Fetch external products '{Type}'", typeof(TExternalProductModel));

        var externalProducts = _deserializer.Deserialize(request);

        await _historyRecorder.RecordHistoryAsync(
            externalProducts
                .Select(x => _historyConverter.Convert(x))
                .ToList())
            .ConfigureAwait(false);

        var products = externalProducts
            .Select(x => _converter.Convert(x))
            .ToList();

        _logger.LogDebug(
            "External products '{Type}' succesfull fetch and converted",
            typeof(TExternalProductModel));

        return products;
    }
}
