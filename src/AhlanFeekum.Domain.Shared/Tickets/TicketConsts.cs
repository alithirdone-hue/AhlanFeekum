namespace AhlanFeekum.Tickets
{
    public static class TicketConsts
    {
        private const string DefaultSorting = "{0}FirstName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Ticket." : string.Empty);
        }

    }
}