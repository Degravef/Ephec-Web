using Dal.Interfaces;
using Dal.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.Extensions;

public static class InjectServices
{
    public static IServiceCollection AddDalServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}