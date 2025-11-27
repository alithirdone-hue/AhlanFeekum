namespace AhlanFeekum.UserPayments
{
    public static class UserPaymentConsts
    {
        private const string DefaultSorting = "{0}Amount asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "UserPayment." : string.Empty);
        }

    }
}