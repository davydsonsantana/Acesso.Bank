using System;

namespace Acesso.Application.Bank.ViewModel {
    public class AccountGetRequestVM {
        public int id { get; set; }

        public string accountNumber { get; set; }

        public float balance { get; set; }
    }
}
