using Import.Logic;
using Import.Logic.Abstractions.Commands;
using Import.Logic.Commands;

namespace Import;

public static class Registrations
{
    public static IServiceCollection AddImportServices(this IServiceCollection services)
        => services
            .AddHostedServices()
            .AddLogic();

    private static IServiceCollection AddLogic(this IServiceCollection services) 
        => services
            .AddSingleton<ICommandFactory, CommandFactory>()
            .AddSingleton<Func<SetLinkCommandParameters, ICommand>>(
               static provider => (parameters) => ActivatorUtilities.CreateInstance<SetLinkCommand>(provider, parameters));

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
        => services.AddHostedService<CacheInizializerService>();
}
