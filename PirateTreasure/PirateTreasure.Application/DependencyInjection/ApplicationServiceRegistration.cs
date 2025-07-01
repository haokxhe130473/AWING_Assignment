using Microsoft.Extensions.DependencyInjection;

namespace PirateTreasure.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Đăng ký MediatR, AutoMapper, FluentValidation,... nếu có
            // services.AddMediatR(...);
            // services.AddAutoMapper(...);

            return services;
        }
    }
}