namespace AhlanFeekum.AhlanfeekumTerms
{
    public static class AhlanfeekumTermConsts
    {
        private const string DefaultSorting = "{0}TermsTitle asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "AhlanfeekumTerm." : string.Empty);
        }

    }
}