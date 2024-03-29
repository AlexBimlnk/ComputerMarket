﻿using General.Logic;
using General.Logic.Commands;
using General.Logic.Executables;
using General.Logic.Queries;
using General.Storage;
using General.Transport;

using Import.Logic;
using Import.Logic.Abstractions;
using Import.Logic.Commands;
using Import.Logic.Models;
using Import.Logic.Queries;
using Import.Logic.Storage.Repositories;
using Import.Logic.Transport.Configuration;
using Import.Logic.Transport.Deserializers;
using Import.Logic.Transport.Models;
using Import.Logic.Transport.Receivers;
using Import.Logic.Transport.Senders;
using Import.Logic.Transport.Serializers;

using Microsoft.Extensions.Options;

namespace Import;

public static class Registrations
{
    public static IServiceCollection AddImportServices(this IServiceCollection services, IConfiguration configuration) =>
        services
        .AddConfigurations(configuration)
        .AddHostedServices()
        .AddLogic()
        .AddTransport()
        .AddStorage();

    private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<InternalProductSenderConfiguration>(configuration.GetSection(nameof(InternalProductSenderConfiguration)))
            .AddSingleton<IValidateOptions<InternalProductSenderConfiguration>, SenderConfigurationValidator<InternalProductSenderConfiguration>>();

    private static IServiceCollection AddLogic(this IServiceCollection services)
        => services
            .AddSingleton<IMapper<Product>, Mapper>()
            .AddSingleton<IHistoryRecorder, HistoryRecorder>()

            .AddScoped<IAPIQueryHandler, APIQueryHandler>()
            .AddScoped<IAPICommandHandler, APICommandHandler>()
            .AddScoped(typeof(IAPIRequestHandler<>), typeof(APIExternalProductsHandler<>))

            .AddSingleton<ProductsConverter>()
            .AddSingleton<IConverter<ExternalProduct, Product>>(sp =>
                sp.GetRequiredService<ProductsConverter>())
            .AddSingleton<IConverter<HornsAndHoovesProduct, Product>>(sp =>
                sp.GetRequiredService<ProductsConverter>())

            .AddScoped<IQueryFactory, QueryFactory>()
            .AddScoped<Func<GetLinksQueryParameters, IQuery>>(
                static provider => (parameters) => ActivatorUtilities.CreateInstance<GetLinksQuery>(provider, parameters))

            .AddScoped<ICommandFactory, CommandFactory>()
            .AddScoped<Func<SetLinkCommandParameters, ICommand>>(
               static provider => (parameters) => ActivatorUtilities.CreateInstance<SetLinkCommand>(provider, parameters))
            .AddScoped<Func<DeleteLinkCommandParameters, ICommand>>(
               static provider => (parameters) => ActivatorUtilities.CreateInstance<DeleteLinkCommand>(provider, parameters));

    private static IServiceCollection AddTransport(this IServiceCollection services)
        => services
            .AddSingleton<IDeserializer<string, QueryParametersBase>, QueryParametersDeserializer>()
            .AddSingleton<IDeserializer<string, CommandParametersBase>, CommandParametersDeserializer>()
            .AddSingleton<IDeserializer<string, IReadOnlyCollection<ExternalProduct>>, IvanovProductsDeserializer>()
            .AddSingleton<IDeserializer<string, IReadOnlyCollection<HornsAndHoovesProduct>>, HornsAndHoovesProductsDeserializer>()

            .AddSingleton<ISerializer<IReadOnlyCollection<Product>, string>, ProductsSerializer>()

            .AddSingleton(typeof(IAPIProductFetcher<>), typeof(APIProductFetcher<>))

            .AddSingleton<ISender<InternalProductSenderConfiguration, IReadOnlyCollection<Product>>, APIInternalProductSender>();

    private static IServiceCollection AddStorage(this IServiceCollection services)
        => services
            .AddScoped<IRepositoryContext, RepositoryContext>()
            .AddScoped<IRepository<Link>, LinkRepository>()
            .AddScoped<IRepository<History>, HistoryRepository>()

            .AddSingleton<Cache>()
            .AddSingleton<IKeyableCache<Link, ExternalID>>(sp => sp.GetRequiredService<Cache>())
            .AddSingleton<ICache<Link>>(sp => sp.GetRequiredService<Cache>());

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
        => services.AddHostedService<CacheInizializerService>();
}
