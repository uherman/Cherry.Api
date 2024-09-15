using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Driving;

public static class ConfigureServices
{
    public static void AddDriving(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
    }
}