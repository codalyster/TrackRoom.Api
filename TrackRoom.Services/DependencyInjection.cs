using Microsoft.Extensions.DependencyInjection;
using TrackRoom.Services.Interfaces;
using TrackRoom.Services.Services;

namespace TrackRoom.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOtpService, OtpService>();
            return services;
        }
    }
}
