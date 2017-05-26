using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTransaction.Domain
{
    public class User : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Decimal Amount { get; set; }
        public IList<Transaction> Transactions { get; set; }


    }
}
