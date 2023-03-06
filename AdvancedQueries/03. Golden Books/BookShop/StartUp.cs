namespace BookShop;

using Data;
using Initializer;
using Models.Enums;

public class StartUp
{
    public static void Main()
    {
        using var db = new BookShopContext();
        // DbInitializer.ResetDatabase(db); 

        // 02
        //  string ageRestrictionInput = Console.ReadLine();
        // string result = GetBooksByAgeRestriction(db, ageRestrictionInput);

        // 03
        string result = GetGoldenBooks(db);

        Console.WriteLine(result);
    }

    // 02
    public static string GetBooksByAgeRestriction(BookShopContext context, string command)
    {
        bool hasParsed = Enum.TryParse(typeof(AgeRestriction), command, true, out object ageRestrictionObj);

        AgeRestriction ageRestriction;

        if (hasParsed)
        {
            ageRestriction = (AgeRestriction)ageRestrictionObj;


            string[] books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        return null;
    }

    // 03
    public static string GetGoldenBooks(BookShopContext dbContext)
    {
        string[] books = dbContext.Books
            .Where(b => b.EditionType == EditionType.Gold &&
                        b.Copies < 5000)
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToArray();


        return string.Join(Environment.NewLine, books);
    }
}
