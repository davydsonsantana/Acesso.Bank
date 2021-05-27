using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acesso.Domain.Bank.Interfaces.Mongo {
    public interface IMongoDbSettings {

        string CnStringFull { get;  }
        string DbHost { get; set; }
        string DbName { get; set; }
        string DbUser { get; set; }
        string DbPass { get; set; }
    }
}
