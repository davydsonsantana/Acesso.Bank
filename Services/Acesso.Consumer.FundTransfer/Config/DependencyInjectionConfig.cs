using Acesso.Infra.CrossCutting.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Acesso.Consumer.FundTransfer.Config {
    public static class DependencyInjectionConfig {

        public static void AddDependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            NativeInjectorBootStrapper.RegisterServices(configuration, services);
        }
    }
}
