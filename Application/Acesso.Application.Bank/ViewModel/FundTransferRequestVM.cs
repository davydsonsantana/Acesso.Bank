using System;
using System.ComponentModel.DataAnnotations;

namespace Acesso.Application.Bank.ViewModel {
    public class FundTransferRequestVM {
        
        public string accountOrigin { get; set; }
        public string accountDestination { get; set; }
        public float value { get; set; }
    }
}
