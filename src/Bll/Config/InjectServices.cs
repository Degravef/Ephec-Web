using Bll.Interfaces;
using Bll.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bll.Config;

public static class InjectServices
{
    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICourseService, CourseService>();
        return services;
    }
}