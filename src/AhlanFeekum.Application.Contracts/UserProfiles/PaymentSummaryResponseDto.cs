using System.Collections.Generic;

namespace AhlanFeekum.UserProfiles
{
    public class PaymentSummaryResponseDto
    {
        /// <summary>
        /// Monthly payments grouped by year-month (e.g., "2024-01", "2024-02")
        /// Key: Month in format "YYYY-MM"
        /// Value: Total amount for that month
        /// </summary>
        public Dictionary<string, decimal> MonthlyPayments { get; set; } = new Dictionary<string, decimal>();

        /// <summary>
        /// Total payment amount across all months in the date range
        /// </summary>
        public decimal TotalPayment { get; set; }

        /// <summary>
        /// Currency code (e.g., "usd", "eur")
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Number of successful payments
        /// </summary>
        public int PaymentCount { get; set; }
    }
}

