using System;

namespace Acesso.Application.Bank.ViewModel {
    public class BalanceAdjustmentRequestVM {
        public string accountNumber { get; set; }

        public float value { get; set; }

        public string type { get; set; }

    }
}
