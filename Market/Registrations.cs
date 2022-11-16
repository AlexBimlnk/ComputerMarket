using General.Storage;

using Market.Logic;
using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

namespace Market;

public static class Registrations
{
    public static IServiceCollection AddMarketServices(this IServiceCollection services, IConfiguration configuration) =>
        services
        .AddStorage();


    private static IServiceCollection AddStorage(this IServiceCollection services)
        => services
            .AddScoped<IRepositoryContext, RepositoryContext>()
            .AddScoped<IUsersRepository, UsersRepository>();
}
