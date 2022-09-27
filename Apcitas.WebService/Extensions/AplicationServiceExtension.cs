using Apcitas.WebService.Data;
using Apcitas.WebService.Interfaces;
using Apcitas.WebService.Services;
using Microsoft.EntityFrameworkCore;

namespace Apcitas.WebService.Extensions;

public static class AplicationServiceExtension
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<iTokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(
                config.GetConnectionString("DefaultConnection")
                );
        });
        
        return services;
    } 
}
