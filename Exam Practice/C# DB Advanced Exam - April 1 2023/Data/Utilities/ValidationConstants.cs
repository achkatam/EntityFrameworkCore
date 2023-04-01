namespace Boardgames.Data.Utilities;


public static class ValidationConstants
{
    // Creator
    public const int FIRST_NAME_MIN = 2;
    public const int FIRST_NAME_MAX = 7; 
    public const int LAST_NAME_MIN = 2;
    public const int LAST_NAME_MAX = 7;

    // Boardgame
    public const int BOARDGAME_NAME_MIN = 10;
    public const int BOARDGAME_NAME_MAX = 20;

    public const double RATING_MAX = 10.00;
    public const double RATING_MIN = 1.00;

    public const int YEAR_MIN = 2018;
    public const int YEAR_MAX = 2023;

    // Seller
    public const int SELLER_NAME_MIN = 5;
    public const int SELLER_NAME_MAX = 20;

    public const int ADDRESS_MIN = 2;
    public const int ADDRESS_MAX = 30;

    public const string REGEX_PATTERN = @"^www\.[a-zA-Z0-9\-]+\.com$";
}