using Acesso.Application.Bank.ViewModel;
using Acesso.Domain.Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acesso.Application.Bank.Interface {
    public interface IFundTransferService : IDisposable {

        FundTransferResultVM SendTransferRequestToQueue(FundTransferRequestVM fundTransferVM);

        void ProcessFundTransferQueue();

        FundTransferStatus GetFundTransferStatus(string transactionId);

    }
}
