using Acesso.Application.Bank.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Acesso.Application.Bank.Interface {
    public interface IAccountHttpClientService : IDisposable {

        Task<AccountGetRequestVM> Get(string accountNumber);

        Task<HttpResponseMessage> TransactionRequest(BalanceAdjustmentRequestVM transaction);
    }
}
