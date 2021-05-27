using Acesso.Consumer.FundTransfer.Config;
using Acesso.Infra.Data.Repository.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acesso.Consumer.FundTransfer {
    public class Program {
        public static void Main(string[] args) {            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => {                    
                    // DI Abstraction
                    services.AddDependencyInjectionConfiguration(hostContext.Configuration);

                    services.AddHostedService<Worker>();
                });
    }
}
