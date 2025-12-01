using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlanFeekum.Reservations
{
    public enum ReservationStatus
    {
        Pending = 1,
        Approved = 2,
        NotAvailable = 3,
        Rejected = 4,
        Canceled = 4,
        Confirmed = 5,
    }
}
