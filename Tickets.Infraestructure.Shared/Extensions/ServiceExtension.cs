using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tickets.Aplication.Interfaces.Email;
using Tickets.Infraestructure.Shared.Services;

namespace Tickets.Infraestructure.Shared.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddSharedInfraestructure(this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            services.AddTransient<IEmailService, EmailService>();
            return services;
        }
    }
}
