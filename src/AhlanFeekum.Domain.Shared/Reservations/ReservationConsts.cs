namespace AhlanFeekum.Reservations
{
    public static class ReservationConsts
    {
        private const string DefaultSorting = "{0}FromeDate asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Reservation." : string.Empty);
        }

    }
}