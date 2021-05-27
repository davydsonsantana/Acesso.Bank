using Acesso.Application.Bank.ViewModel;
using System.Net.Http;
using Acesso.Infra.Helper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Acesso.Application.Bank.Interface;

namespace Acesso.Consumer.FundTransfer {
    public class Worker : BackgroundService {

        private readonly ILogger<Worker> _logger;
        private readonly IFundTransferService _fundTransfer;
        private readonly HttpClient httpClient;

        public Worker(ILogger<Worker> logger, IFundTransferService fundTransfer) {
            _logger = logger;
            _fundTransfer = fundTransfer;
            httpClient = new HttpClient();
            SetupHttpClient();
        }

        private void SetupHttpClient() {
            httpClient.Timeout = TimeSpan.FromMilliseconds(1000);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
                        
            try {
                _logger.LogInformation("Acesso.Consumer.FundTransfer Started: {time}", DateTimeOffset.Now);
                _fundTransfer.ProcessFundTransferQueue();
            }catch (Exception ex) {

            }


        }        
    
    }
}
