using Acesso.Application.Bank.ViewModel;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Acesso.Application.Bank.Interface;
using RabbitMQ.Client.Events;
using System.Net.Http.Json;
using System.Net.Http;

namespace Acesso.Application.Bank.Service {
    public class AccountHttpClientService : IAccountHttpClientService {

        private readonly HttpClient _httpClient;

        public AccountHttpClientService(HttpClient httpClient) {
            _httpClient = httpClient;
        }
                
        public async Task<AccountVM> Get(string accountNumber) {
            string url = "http://services.accountapi/api/Account/" + accountNumber;
            return await _httpClient.GetFromJsonAsync<AccountVM>(url);
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
        }
    }
}
