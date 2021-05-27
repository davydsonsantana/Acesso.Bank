using Acesso.Domain.Bank.Interfaces.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acesso.Infra.Data.Repository.Mongo {
    public class MongoDbSettings : IMongoDbSettings {

        public string CnStringFull {
            get {
                return $"mongodb://{DbUser}:{DbPass}@{DbHost}";
            }
        }
        public string DbHost { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPass { get; set; }



    }
}
