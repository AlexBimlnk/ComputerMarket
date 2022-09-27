using Import.Logic.Abstractions.Commands;
using Import.Logic.Commands;

namespace Import;

public static class Registrations
{
    public static IServiceCollection AddLogic(this IServiceCollection services) 
        => services
            .AddSingleton<ICommandFactory, CommandFactory>()
            .AddSingleton<Func<SetLinkCommandParameters, ICommand>>(
                provider => (parameters) => ActivatorUtilities.CreateInstance<SetLinkCommand>(provider, parameters));
}
