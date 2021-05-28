using Acesso.Domain.Bank.Interfaces;
using Acesso.Domain.Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acesso.Infra.Data.Repository {
        
    public class FundTransferStatusRepository : IFundTransferStatusRepository {

        private readonly IMongoRepository<FundTransferStatus> _fundTransferStatus;

        public FundTransferStatusRepository(IMongoRepository<FundTransferStatus> fundTransferStatus) {
            _fundTransferStatus = fundTransferStatus;
        }

        public void Add(FundTransferStatus fundTransferStatus) {
            _fundTransferStatus.InsertOne(fundTransferStatus);            
        }

        public void Replace(FundTransferStatus fundTransferStatus) {
            _fundTransferStatus.ReplaceOne(fundTransferStatus);
        }

        public FundTransferStatus GetByTransactionId(string transactionId) {
            return _fundTransferStatus.FindOne(x => x.TransactionId == transactionId);
        }

        public void Remove(FundTransferStatus fundTransferStatus) {
            _fundTransferStatus.DeleteById(fundTransferStatus.Id.ToString());
        }

        public void Update(FundTransferStatus fundTransferStatus) {
            _fundTransferStatus.ReplaceOne(fundTransferStatus);
        }

   
    }
}
