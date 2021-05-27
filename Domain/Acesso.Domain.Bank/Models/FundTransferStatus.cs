using Acesso.Domain.Bank.Interfaces;
using Acesso.Domain.Bank.Models.Mongo;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acesso.Domain.Bank.Models {
    [BsonCollection("FundTransferStatus")]
    public class FundTransferStatus : Document {

        public FundTransferStatus() { }

        public string TransactionId { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }

    }
}
