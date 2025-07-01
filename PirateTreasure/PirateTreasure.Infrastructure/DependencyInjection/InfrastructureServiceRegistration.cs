using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PirateTreasure.Application.Contracts.Persistence;
using PirateTreasure.Infrastructure.Persistence;
using PirateTreasure.Infrastructure.Persistence.Repositories;

namespace PirateTreasure.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký repository
            services.AddScoped<ITreasureMapRepository, TreasureMapRepository>();
            services.AddScoped<ITreasureCellRepository, TreasureCellRepository>();

            // Đăng ký các dịch vụ hạ tầng khác (nếu có)
            // services.AddScoped<IFileStorage, LocalFileStorage>();

            return services;
        }
    }
}