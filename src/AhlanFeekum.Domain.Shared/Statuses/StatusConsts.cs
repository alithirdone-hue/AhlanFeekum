namespace AhlanFeekum.Statuses
{
    public static class StatusConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Status." : string.Empty);
        }

    }
}