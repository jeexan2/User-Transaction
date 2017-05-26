using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTransaction.Domain
{
    public class Transaction: Base
    {
        public decimal Amount { get; set; }
        public DateTime DateTransaction { get; set; }
        public int UserId { get; set; }
        public Transaction()
        {
            
        }
    }
}
