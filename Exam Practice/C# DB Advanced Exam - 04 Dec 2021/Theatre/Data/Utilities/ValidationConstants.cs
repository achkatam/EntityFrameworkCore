namespace Theatre.Data.Utilities;

public static class ValidationConstants
{
    // Theater
    public const int THEATER_NAME_MAX = 30;
    public const int THEATER_NAME_MIN = 4;

    public const int NUMBER_OF_HALLS_MIN = 1;
    public const int NUMBER_OF_HALLS_MAX = 10;

    public const int DIRECTOR_MIN = 4;
    public const int DIRECTOR_MAX = 30;

    // Cast
    public const int FULLNAME_NAME_MAX = 30;
    public const int FULLNAME_NAME_MIN = 4;

    public const string REGEX_PATTERN = @"[+44]{3}-[\d]{2}-[\d]{3}-[\d]{4}";

    // Play
    public const int PLAY_TITLE_MAX = 50;
    public const int PLAY_TITLE_MIN = 4;

    public const int DESCRIPTION_MAX = 700;
    public const int SCREENWRITER_MAX = 50;
    public const int SCREENWRITER_MIN = 4;
}