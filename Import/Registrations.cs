using General.Logic;
using General.Storage;
using General.Transport;

using Import.Logic;
using Import.Logic.Abstractions;
using Import.Logic.Abstractions.Commands;
using Import.Logic.Commands;
using Import.Logic.Models;
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
    public static IServiceCollection AddImportServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfigurations(configuration);

        return services
            .AddHostedServices()
            .AddLogic()
            .AddTransport()
            .AddStorage();
    }

    private static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<InternalProductSenderConfiguration>(configuration.GetSection(nameof(InternalProductSenderConfiguration)))
            .AddSingleton<IValidateOptions<InternalProductSenderConfiguration>, SenderConfigurationValidator<InternalProductSenderConfiguration>>();

    private static IServiceCollection AddLogic(this IServiceCollection services)
        => services
            .AddSingleton<IMapper<Product>, Mapper>()
            .AddSingleton<IHistoryRecorder, HistoryRecorder>()

            .AddSingleton<IAPICommandHandler, APICommandHandler>()
            .AddSingleton<IAPIRequestHandler<ExternalProduct>, APIExternalProductsHandler<ExternalProduct>>()
            .AddSingleton<IAPIRequestHandler<HornsAndHoovesProduct>, APIExternalProductsHandler<HornsAndHoovesProduct>>()

            .AddSingleton<ProductsConverter>()
            .AddSingleton<IConverter<ExternalProduct, Product>>(sp =>
                sp.GetRequiredService<ProductsConverter>())
            .AddSingleton<IConverter<HornsAndHoovesProduct, Product>>(sp =>
                sp.GetRequiredService<ProductsConverter>())

            .AddSingleton<ICommandFactory, CommandFactory>()
            .AddSingleton<Func<SetLinkCommandParameters, ICommand>>(
               static provider => (parameters) => ActivatorUtilities.CreateInstance<SetLinkCommand>(provider, parameters))
            .AddSingleton<Func<DeleteLinkCommandParameters, ICommand>>(
               static provider => (parameters) => ActivatorUtilities.CreateInstance<DeleteLinkCommand>(provider, parameters));

    private static IServiceCollection AddTransport(this IServiceCollection services)
        => services
            .AddSingleton<IDeserializer<string, CommandParametersBase>, CommandParametersDeserializer>()
            .AddSingleton<IDeserializer<string, IReadOnlyCollection<ExternalProduct>>, ExternalProductsDeserializer>()
            .AddSingleton<IDeserializer<string, IReadOnlyCollection<HornsAndHoovesProduct>>, HornsAndHoovesProductsDeserializer>()

            .AddSingleton<ISerializer<IReadOnlyCollection<Product>, string>, ProductsSerializer>()

            .AddSingleton<IAPIProductFetcher<ExternalProduct>, APIProductFetcher<ExternalProduct>>()
            .AddSingleton<IAPIProductFetcher<HornsAndHoovesProduct>, APIProductFetcher<HornsAndHoovesProduct>>()

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
