using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Acesso.Application.Bank.Interface;
using Acesso.Application.Bank.Service;
using System;
using Polly.Extensions.Http;
using Polly;

namespace Acesso.Infra.CrossCutting.IoC {
    public static class NativeInjectorBootStrapper {

        public static void RegisterServices(IServiceCollection services) {

            // Application
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IFundTransferService, FundTransferService>();

            // Http Policy
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(10));

            // Http Client
            services.AddHttpClient<IAccountHttpClientService, AccountHttpClientService>()
                .AddPolicyHandler(retryPolicy);
        }        

    }
}
