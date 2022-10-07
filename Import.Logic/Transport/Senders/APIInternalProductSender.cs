using Import.Logic.Abstractions;
using Import.Logic.Models;
using Import.Logic.Transport.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Import.Logic.Transport.Senders;

/// <summary xml:lang = "ru">
/// Отправитель внутренних продуктов по API.
/// </summary>
public class APIInternalProductSender : ISender<InternalProductSenderConfiguration, IReadOnlyCollection<Product>>
{
    private readonly ILogger<APIInternalProductSender> _logger;
    private readonly InternalProductSenderConfiguration _configuration;
    private readonly ISerializer<IReadOnlyCollection<Product>, string> _serializer;

    public APIInternalProductSender(
        ILogger<APIInternalProductSender> logger,
        IOptions<InternalProductSenderConfiguration> options,
        ISerializer<IReadOnlyCollection<Product>, string> serializer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <inheritdoc/>
    public async Task SendAsync(IReadOnlyCollection<Product> products, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(products, nameof(products));

        token.ThrowIfCancellationRequested();

        if (!products.Any())
        {
            _logger.LogWarning("Sender given empty collection");
            return;
        }

        _logger.LogDebug("Sending products: {Products}", string.Join(", ", products));

        var request = _serializer.Serialize(products);
        using var content = new StringContent(request);

        using var client = new HttpClient();

        HttpResponseMessage response = null!;

        try
        {
            response = await client.PostAsync(_configuration.Destination, content, token);

            if (response.IsSuccessStatusCode)
                _logger.LogInformation("The products have been successfully sended");
            else
                _logger.LogWarning(
                    "The products have not been sended. Response status code: {Status code}",
                    response.StatusCode);
        }
        catch (Exception ex) 
            when (ex is InvalidOperationException or HttpRequestException)
        {
            _logger.LogWarning(
                "The products have not been sended. More info: {Info}",
                ex.Message);
        }
        finally
        {
            response?.Dispose();
        }
    }
}
