namespace AhlanFeekum.CashPayments
{
    public static class CashPaymentConsts
    {
        private const string DefaultSorting = "{0}Amount asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "CashPayment." : string.Empty);
        }

    }
}