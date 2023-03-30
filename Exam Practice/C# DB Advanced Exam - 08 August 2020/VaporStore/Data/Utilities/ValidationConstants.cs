namespace VaporStore.Data.Utilities
{
    using System.Text.RegularExpressions;

    public static class ValidationConstants
    {
        // User
        public const int USERNAME_MIN = 3;
        public const int USERNAME_MAX = 20;

        public const string FULLNAME_PATTERN = @"^[A-Z][a-z]+ [A-Z][a-z]+$";
        public const int AGE_MIN = 3;
        public const int AGE_MAX = 103;

        // Card
        public const string CARD_REGEX = @"(\d{4}) (\d{4}) (\d{4}) (\d{4})";
        public const string CARD_CVC = @"(\d{3})";

        // Purchase
        public const string PURCHASE_REGEX = @"^[A-Z\d]{4}-[A-Z\d]{4}-[A-Z\d]{4}$";
    }
}