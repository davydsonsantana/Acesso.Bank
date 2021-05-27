using System;
using System.ComponentModel.DataAnnotations;

namespace Acesso.Application.Bank.ViewModel {
    public class FundTransferQueueVM {

        public string transactionId { get; set; }
        public string accountOrigin { get; set; }
        public string accountDestination { get; set; }
        public float value { get; set; }

        public FundTransferQueueVM() {

        }

        public FundTransferQueueVM(string transactionId, FundTransferRequestVM fundTransferRequest) {
            this.transactionId = transactionId;
            accountOrigin = fundTransferRequest.accountOrigin;
            accountDestination = fundTransferRequest.accountDestination;
            value = fundTransferRequest.value;
        }

    }
}
