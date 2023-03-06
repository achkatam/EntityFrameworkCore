using System.Globalization;
using System.Net.Http.Headers;

namespace BookShop;

using System.Text;
using BookShop.Models;
using Data;
using Initializer;
using Models.Enums;
using static System.Reflection.Metadata.BlobBuilder;

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
        // string result = GetGoldenBooks(db);

        //04
        // string result = GetBooksByPrice(db);

        // 05
        //  int releaseYear = int.Parse(Console.ReadLine());
        //  string result = GetBooksNotReleasedIn(db, releaseYear);

        // 06
        //string input = Console.ReadLine();
        //string result = GetBooksByCategory(db, input);

        // 07
        //string date = Console.ReadLine();
        //string result = GetBooksReleasedBefore(db, date);

        // 08
        //string input = Console.ReadLine();
        //string result = GetAuthorNamesEndingIn(db, input);

        // 09 Book Search
        // string input = Console.ReadLine();
        // string result = GetBookTitlesContaining(db, input);

        // 10.	Book Search by Author
        //string input = Console.ReadLine();
        //string result = GetBooksByAuthor(db, input);

        // 11. Count Books
        //int numberOfChars = int.Parse(Console.ReadLine());
        //int result = CountBooks(db, numberOfChars);

        // 12. Total Book Copies
        // string result = CountCopiesByAuthor(db);

        // 13. Profit by Category
        string result = GetTotalProfitByCategory(db);

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

    // 04
    public static string GetBooksByPrice(BookShopContext context)
    {
        string[] books = context.Books
            .Where(b => b.Price > 40)
            .OrderByDescending(b => b.Price)
            .Select(b => $"{b.Title} - ${b.Price:f2}")
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    // 05
    public static string GetBooksNotReleasedIn(BookShopContext context, int year)
    {
        string[] books = context.Books
            .Where(b => b.ReleaseDate.Value.Year != year)
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    // 06
    public static string GetBooksByCategory(BookShopContext dbContext, string input)
    {
        string[] categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(c => c.ToLower())
            .ToArray();

        string[] books = dbContext.Books
            .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
            .OrderBy(b => b.Title)
            .Select(b => b.Title)
            .ToArray();


        return string.Join(Environment.NewLine, books);
    }
    // 07
    public static string GetBooksReleasedBefore(BookShopContext dbContext, string date)
    {
        var dateFormat = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

        string[] books = dbContext.Books
            .Where(b => b.ReleaseDate < dateFormat)
            .OrderByDescending(b => b.ReleaseDate)
            .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    // 08
    public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
    {
        string[] authors = dbContext.Authors
            .Where(a => a.FirstName.EndsWith(input))
            .OrderBy(a => a)
            .Select(a => a.FirstName + " " + a.LastName)
            .ToArray();

        return string.Join(Environment.NewLine, authors);
    }

    // 09 Book Search
    public static string GetBookTitlesContaining(BookShopContext dbContext, string input)
    {
        string[] books = dbContext.Books
            .Where(b => b.Title.ToLower().Contains(input.ToLower()))
            .OrderBy(b => b.Title)
            .Select(b => b.Title)
        .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    // 10.	Book Search by Author
    public static string GetBooksByAuthor(BookShopContext dbContext, string input)
    {
        string[] books = dbContext.Books
            .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
            .OrderBy(b => b.BookId)
            .Select(b => $"{b.Title} ({b.Author.FirstName + " " + b.Author.LastName})")
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    // 11. Count Books
    public static int CountBooks(BookShopContext dbContext, int lengthCheck)
    {
        string[] books = dbContext.Books
            .Where(b => b.Title.Length > lengthCheck)
            .Select(b => b.Title)
            .ToArray();

        return books.Count();
    }

    // 12. Total Book Copies
    public static string CountCopiesByAuthor(BookShopContext dbContext)
    {
        StringBuilder sb = new StringBuilder();

        var books = dbContext.Authors
            .Select(a => new
            {
                AuthorName = $"{a.FirstName} {a.LastName}",
                TotalCopies = a.Books.Sum(x => x.Copies)
            })
            .OrderByDescending(x => x.TotalCopies)
            .ToArray();

        foreach (var book in books)
        {
            sb
                .AppendLine($"{book.AuthorName} - {book.TotalCopies}");
        }

        return sb.ToString().TrimEnd();
    }

    // 13. Profit by Category
    public static string GetTotalProfitByCategory(BookShopContext dbContext)
    {
        StringBuilder sb = new StringBuilder();

        var profits = dbContext.Categories
            .Select(c => new
            {
                Name = c.Name,
                Profit = c.CategoryBooks.Sum(cb => cb.Book.Price * cb.Book.Copies)
            })
            .OrderByDescending(c => c.Profit)
            .ThenBy(c => c.Name)
            .ToArray();

        foreach (var profit in profits)
        {
            sb
                .AppendLine($"{profit.Name} ${profit.Profit:f2}");
        }

        return sb.ToString().TrimEnd();
    }
}
