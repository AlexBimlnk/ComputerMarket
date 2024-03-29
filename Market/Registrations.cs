﻿using General.Logic;
using General.Storage;
using General.Transport;

using Market.Logic;
using Market.Logic.Commands;
using Market.Logic.Commands.Import;
using Market.Logic.ComputerBuilder;
using Market.Logic.Markers;
using Market.Logic.Models;
using Market.Logic.Models.Abstractions;
using Market.Logic.Models.WT;
using Market.Logic.Queries;
using Market.Logic.Queries.Import;
using Market.Logic.Reports;
using Market.Logic.Storage.Repositories;
using Market.Logic.Transport.Configuration;
using Market.Logic.Transport.Configurations;
using Market.Logic.Transport.Deserializers;
using Market.Logic.Transport.Models;
using Market.Logic.Transport.Senders;
using Market.Logic.Transport.Serializers;

using Microsoft.Extensions.Options;

namespace Market;

public static class Registrations
{
    public static IServiceCollection AddMarketServices(this IServiceCollection services, IConfiguration configuration) =>
        services
        .AddStorage()
        .AddConfigurations(configuration)
        .AddLogic()
        .AddTransport();

    private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<ImportCommandConfigurationSender>(configuration.GetSection(nameof(ImportCommandConfigurationSender)))
            .Configure<WTCommandConfigurationSender>(configuration.GetSection(nameof(WTCommandConfigurationSender)))
            .AddSingleton<IValidateOptions<ImportCommandConfigurationSender>, SenderConfigurationValidator<ImportCommandConfigurationSender>>()
            .AddSingleton<IValidateOptions<WTCommandConfigurationSender>, SenderConfigurationValidator<WTCommandConfigurationSender>>();

    private static IServiceCollection AddLogic(this IServiceCollection services)
        => services
            .AddScoped<IReportBuilder, ReportBuilder>()
            .AddScoped<IAPIRequestHandler<ImportMarker>, ImportProductsHandler>()
            .AddScoped<IAPIRequestHandler<WTMarker>, TransactionRequestHandler>()
            .AddScoped<ICatalog, Catalog>()
            .AddScoped<IComputerBuilder, ComputerBuilder>();

    private static IServiceCollection AddStorage(this IServiceCollection services)
        => services
            .AddScoped<IRepositoryContext, RepositoryContext>()
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IProvidersRepository, ProvidersRepository>()
            .AddScoped<ProductsRepository>()
            .AddScoped<IProductsRepository>(x => x.GetRequiredService<ProductsRepository>())
            .AddScoped<IItemsRepository>(x => x.GetRequiredService<ProductsRepository>())
            .AddScoped<IOrderRepository, OrdersRepository>()
            .AddScoped<IBasketRepository, BasketRepository>()
            .AddScoped<IComputerBuildRulesRepository, RulesRepository>();

    private static IServiceCollection AddTransport(this IServiceCollection services)
        => services
            .AddSingleton<IDeserializer<string, TransactionRequestResult>, TransactionResultDeserializer>()

            .AddSingleton<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>, ProductDeserializer>()
            .AddSingleton<IDeserializer<string, CommandResult>, CommandResultDeserializer>()
            .AddSingleton<IDeserializer<string, QueryResult<IReadOnlyCollection<Link>>>, ImportQueryResultDeserializer>()

            .AddSingleton<ISerializer<ImportQuery, string>, ImportQuerySerializer>()
            .AddSingleton<ISerializer<ImportCommand, string>, ImportCommandSerializer>()
            .AddSingleton<ISerializer<WTCommand, string>, WTCommandSerializer>()

            .AddSingleton(typeof(IQuerySender<,,>), typeof(QuerySender<,,>))
            .AddSingleton(typeof(ISender<,>), typeof(CommandSender<,>));
}
