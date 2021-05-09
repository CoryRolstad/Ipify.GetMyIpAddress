
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Ipify.GetMyIpAddress
{
    public static class IpServiceCollectionExtensions
    {
        public static IServiceCollection AddIpService(this IServiceCollection services, IpServiceSettings ipServiceSettings)
        {
            ValidateSettings(services, ipServiceSettings);
            services.AddSingleton(ipServiceSettings);
            services.AddHttpClient();
            services.AddTransient<IIpService, IpService>();
            return services;
        }

        private static void ValidateSettings(IServiceCollection services, IpServiceSettings ipServiceSettings)
        {
            if (services == null) throw new ArgumentNullException($"{nameof(services)} cannot be null");
        }
    }
}
