using Microsoft.Extensions.DependencyInjection;
using Acesso.Application.Bank.Interface;
using Acesso.Application.Bank.Service;
using System;

namespace Acesso.Infra.CrossCutting.IoC {
    public static class NativeInjectorBootStrapper {

        public static void RegisterServices(IServiceCollection services) {

            // Application
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IFundTransferService, FundTransferService>();

        }        

    }
}
