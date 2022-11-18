using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using General.Logic;
using General.Storage;
using General.Transport;

using Market.Logic.Models;

using Microsoft.Extensions.Logging;

namespace Market.Logic.Receivers;
public sealed class ImportProductsHandler : IAPIRequestHandler<IReadOnlyCollection<Product>>
{
    private readonly ILogger<ImportProductsHandler> _logger;
    private readonly IDeserializer<string, IReadOnlyCollection<Product>> _deserializer;
    private readonly IRepository<Product> _repositoryProduct;

    public ImportProductsHandler(
        IDeserializer<string, IReadOnlyCollection<Product>> deserializer, 
        IRepository<Product> repositoryProduct,
        ILogger<ImportProductsHandler> logger)
    {
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        _repositoryProduct = repositoryProduct ?? throw new ArgumentNullException(nameof(repositoryProduct));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(string request, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(request))
        {
            throw new ArgumentException("Source can't be null or empty or contains only whitespaces");
        }

        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Processing new request");

        var products = _deserializer.Deserialize(request);
        
        _logger.LogDebug("Deserialize {Sourse} to Products comlete", request);
        
        foreach (var product in products)
        {
            await _repositoryProduct.AddAsync(product);
        }

        _logger.LogInformation("Requesr processing complete");
    }
}
