using General.Logic.Commands;
using General.Storage;
using General.Transport;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using WalletTransaction.Logic;
using WalletTransaction.Logic.Commands;
using WalletTransaction.Logic.Transport;
using WalletTransaction.Logic.Transport.Configurations;
using WalletTransaction.Logic.Transport.Deserializers;
using WalletTransaction.Logic.Transport.Senders;
using WalletTransaction.Logic.Transport.Serializers;

namespace WalletTransaction;

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
            .Configure<TransactionSenderConfiguration>(configuration.GetSection(nameof(TransactionSenderConfiguration)))
            .Configure<TransactionsResultSenderConfiguration>(configuration.GetSection(nameof(TransactionsResultSenderConfiguration)))
            .AddSingleton<IValidateOptions<TransactionsResultSenderConfiguration>, SenderConfigurationValidator<TransactionsResultSenderConfiguration>>();

    private static IServiceCollection AddLogic(this IServiceCollection services)
        => services
            .AddSingleton<ITransactionRequestExecuter, TransactionRequestExecuter>()
            .AddSingleton<ITransactionsRequestProcessor, TransactionRequestProcessor>()

            .AddScoped<IAPICommandHandler, APICommandHandler>()

            .AddScoped<ICommandFactory, CommandFactory>()
            .AddScoped<Func<CreateTransactionRequestCommandParameters, ICommand>>(
               static provider => (parameters) => ActivatorUtilities.CreateInstance<CreateTransactionRequestCommand>(provider, parameters))
            .AddScoped<Func<FinishTransactionRequestCommandParameters, ICommand>>(
               static provider => (parameters) => ActivatorUtilities.CreateInstance<FinishTransactionRequestCommand>(provider, parameters))
            .AddScoped<Func<CancelTransactionRequestCommandParameters, ICommand>>(
               static provider => (parameters) => ActivatorUtilities.CreateInstance<CancelTransactionRequestCommand>(provider, parameters));

    private static IServiceCollection AddTransport(this IServiceCollection services)
        => services
            .AddSingleton<IDeserializer<string, CommandParametersBase>, CommandParametersDeserializer>()
            .AddSingleton<ISerializer<ITransactionsRequest, string>, TransactionRequestResultSerializer>()

            .AddSingleton<ProcessingTransactionsChannel>()

            .AddSingleton<IReceiver<ITransactionsRequest>>(sp => 
                sp.GetRequiredService<ProcessingTransactionsChannel>())
            .AddSingleton<ISender<TransactionSenderConfiguration, ITransactionsRequest>>(sp =>
                sp.GetRequiredService<ProcessingTransactionsChannel>())

            .AddSingleton<ISender<TransactionsResultSenderConfiguration, ITransactionsRequest>, TransactionsResultSender>();

    private static IServiceCollection AddStorage(this IServiceCollection services)
        => services
            .AddSingleton<TransactionsRequestCache>()
            .AddSingleton<IKeyableCache<TransactionRequest, InternalID>>(sp => sp.GetRequiredService<TransactionsRequestCache>())
            .AddSingleton<ICache<TransactionRequest>>(sp => sp.GetRequiredService<TransactionsRequestCache>());

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
        => services.AddHostedService<RequestService>();
}
