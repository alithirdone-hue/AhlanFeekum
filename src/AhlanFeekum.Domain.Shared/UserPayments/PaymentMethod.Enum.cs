using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlanFeekum.UserPayments
{
    public enum PaymentMethod
    {
        Card = 1,           // Online payment via Stripe
        Cash = 2,           // Cash payment on arrival
        BankTransfer = 3    // Bank transfer
    }
}
