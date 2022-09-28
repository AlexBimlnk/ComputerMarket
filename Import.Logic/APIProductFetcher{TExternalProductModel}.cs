using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Import.Logic;

public sealed class APIProductFetcher<TExternalProductModel> : IProductFetcher
    where TExternalProductModel : class
{
    private readonly IDeserializer<string, TExternalProductModel> _deserializer;
    private readonly ILogger<APIProductFetcher<TExternalProductModel>> _logger;

    public APIProductFetcher(
        ILogger<APIProductFetcher<TExternalProductModel>> logger,
        IDeserializer<string, TExternalProductModel> deserializer)
    {
        _logger = logger;
        _deserializer = deserializer;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Product> FetchProducts() => throw new NotImplementedException();
}
