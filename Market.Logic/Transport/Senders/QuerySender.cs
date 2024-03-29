﻿using General.Transport;

using Market.Logic.Queries;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Market.Logic.Transport.Senders;

/// <summary xml:lang = "ru">
/// Отправитель запросов во внешние сервисы.
/// </summary>
/// <typeparam name="TConfiguration" xml:lang = "ru">
/// Конфигурация отправителя.
/// </typeparam>
/// <typeparam name="TQuery" xml:lang = "ru">
/// Тип команды, которую отправляет отправитель.
/// </typeparam>
public sealed class QuerySender<TConfiguration, TQuery, TResult> : IQuerySender<TConfiguration, TQuery, TResult>
    where TConfiguration : class, ITransportSenderConfiguration
    where TQuery : QueryBase
{
    private readonly ILogger<QuerySender<TConfiguration, TQuery, TResult>> _logger;
    private readonly TConfiguration _configuration;
    private readonly ISerializer<TQuery, string> _serializer;
    private readonly IDeserializer<string, TResult> _deserializer;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="QuerySender"/>
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="options" xml:lang = "ru">
    /// Опции с конфигурацией отправителя.
    /// </param>
    /// <param name="serializer" xml:lang = "ru">
    /// Сериализатор запросов.
    /// </param>
    /// <param name="serializer" xml:lang = "ru">
    /// Десериализатор запросов.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из параметров оказался <see langword="null"/>.
    /// </exception>
    public QuerySender(
        ILogger<QuerySender<TConfiguration, TQuery, TResult>> logger,
        IOptions<TConfiguration> options,
        ISerializer<TQuery, string> serializer,
        IDeserializer<string, TResult> deserializer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options.Value));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
    }

    /// <inheritdoc/>
    public async Task<TResult> SendAsync(TQuery query, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Sending query of type {CommandType}...", typeof(TQuery));

        var request = _serializer.Serialize(query);
        using var content = new StringContent(request);

        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

        using var client = new HttpClient(clientHandler);

        HttpResponseMessage response = null!;

        try
        {
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_configuration.Destination),
                Content = content,
            };

            response = await client.SendAsync(httpRequest)
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                _logger.LogInformation("The query have been successfully sended");
            else
                _logger.LogWarning(
                    "The query have not been sended. Response status code: {Status code}",
                    response.StatusCode);
        }
        catch (Exception ex)
            when (ex is InvalidOperationException or HttpRequestException)
        {
            _logger.LogWarning(
                "The query have not been sended. More info: {Info}",
                ex.Message);
        }

        return _deserializer.Deserialize(await response.Content.ReadAsStringAsync());
    }
}
