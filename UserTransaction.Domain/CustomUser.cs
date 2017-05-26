using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTransaction.Domain
{
    public class CustomUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Decimal Amount { get; set; }
    }
}
