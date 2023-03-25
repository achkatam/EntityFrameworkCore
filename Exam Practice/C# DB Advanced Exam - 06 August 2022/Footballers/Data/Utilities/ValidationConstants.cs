namespace Footballers.Data.Utilities
{
    public static class ValidationConstants
    {
        // Coach
        public const int COACH_NAME_MIN = 2;
        public const int COACH_NAME_MAX = 40;

        // Team
        public const int TEAM_NAME_MIN = 3;
        public const int TEAM_NAME_MAX = 40;
        public const string REGEX_PATTERN = @"[A-Za-z\d\.\- ]{3,40}";

        public const int NATIONALITY_NAME_MIN = 2;
        public const int NATIONALITY_NAME_MAX = 40;

        // Footballer
        public const int FOOTBALLER_NAME_MIN = 2;
        public const int FOOTBALLER_NAME_MAX = 40;
    }
}
