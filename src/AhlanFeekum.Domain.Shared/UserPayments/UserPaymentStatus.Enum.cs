using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlanFeekum.UserPayments
{
    public enum UserPaymentStatus
    {
        Pending = 1,
        succeeded = 2,
        requires_capture = 3,  // Payment authorized, waiting for capture
        canceled = 4,          // Payment canceled
        failed = 5             // Payment failed
    }
}
