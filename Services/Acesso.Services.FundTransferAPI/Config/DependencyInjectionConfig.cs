using Acesso.Infra.CrossCutting.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Acesso.Services.FundTransferAPI.Config {
    public static class DependencyInjectionConfig {

        public static void AddDependencyInjectionConfiguration(this IServiceCollection services) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            NativeInjectorBootStrapper.RegisterServices(services);
        }

    }
}
