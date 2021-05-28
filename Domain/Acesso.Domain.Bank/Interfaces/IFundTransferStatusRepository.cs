using Acesso.Domain.Bank.Models;

namespace Acesso.Domain.Bank.Interfaces {
    public interface IFundTransferStatusRepository {

        FundTransferStatus GetByTransactionId(string transactionId);
        void Add(FundTransferStatus fundTransferStatus);
        void Replace(FundTransferStatus fundTransferStatus);
        void Update(FundTransferStatus fundTransferStatus);
        void Remove(FundTransferStatus fundTransferStatus);

    }
}