using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Acesso.Application.Bank.Interface;
using Acesso.Application.Bank.Service;
using System.Configuration;
using System;
using Polly.Extensions.Http;
using Polly;
using Acesso.Infra.Data.Repository.Mongo;
using Acesso.Domain.Bank.Interfaces;
using Acesso.Infra.Data.Repository;
using Acesso.Domain.Bank.Interfaces.Mongo;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Acesso.Infra.CrossCutting.IoC {
    public static class NativeInjectorBootStrapper {

        public  static void RegisterServices(IConfiguration Configuration, IServiceCollection services) {

            // Mongo Settings
            MongoDbSettings mongoSettings = new MongoDbSettings() {
                DbHost = Configuration["MongoDbSettings:DbHost"],
                DbName = Configuration["MongoDbSettings:DbName"],
                DbUser = Configuration["MongoDbSettings:DbUser"],
                DbPass = Configuration["MongoDbSettings:DbPass"]
            };
            //services.Configure<IMongoDbSettings>(x => Configuration.GetSection("MongoDbSettings"));
            //services.AddSingleton<IMongoDbSettings>(x => x.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            services.AddSingleton<IMongoDbSettings>(x => mongoSettings);            
            services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));            

            // Application
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IFundTransferService, FundTransferService>();

            // Repository
            services.AddSingleton<IFundTransferStatusRepository, FundTransferStatusRepository>();

            // Http Policy
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(10));

            // Http Client
            services.AddHttpClient<IAccountHttpClientService, AccountHttpClientService>()
                .AddPolicyHandler(retryPolicy);

        }

    }
}
