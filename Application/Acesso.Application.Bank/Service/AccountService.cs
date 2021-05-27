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
    public class AccountService : IAccountService {

        private readonly HttpClient _httpClient;

        public AccountService() {
            _httpClient = new HttpClient();
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
        }

        public async Task<AccountVM> Get(string accountNumber) {
            return await _httpClient.GetFromJsonAsync<AccountVM>(
                $"http://localhost:5000/api/account/{accountNumber}"
                );
        }
    }
}
