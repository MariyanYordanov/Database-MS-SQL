namespace BookShop
{
    using Data;
    using System;
    using Initializer;
    using System.Linq;
    using System.Text;
    using BookShop.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            string input = Console.ReadLine();

            Console.WriteLine(GetBookTitlesContaining(context, input));
        }

        // 2.Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            bool parsed = Enum.TryParse(command, true, out AgeRestriction age);

            if (!parsed)
            {
                return String.Empty;
            }

            string[] bookTitles = context.Books
                .Where(b => b.AgeRestriction == age)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return String.Join(Environment.NewLine, bookTitles);
        }

        // 3.Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new { b.Title, b.BookId })
                .OrderBy(b => b.BookId)
                .ToList();

            StringBuilder output = new StringBuilder();

            foreach (var book in books)
            {
                output.AppendLine(book.Title);
            }

            return output.ToString().TrimEnd();
        }

        // 4.Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new { b.Price, b.Title })
                .OrderByDescending(b => b.Price)
                .ToList();

            StringBuilder output = new StringBuilder();

            foreach (var book in books)
            {
                output.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return output.ToString().TrimEnd();
        }

        // 5.Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(t => t)
                .ToArray();

            StringBuilder output = new StringBuilder();

            foreach (var book in books)
            {
                output.AppendLine(book.Title);
            }

            return output.ToString().TrimEnd();
        }

        // 6.Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(' ');

            var bookTitles = context.Books
                .Where(b => b.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(x => x)
                .ToArray();

            return String.Join(Environment.NewLine, bookTitles);
        }

        // 7.Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var targetDate = DateTime
                .ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < targetDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            StringBuilder output = new StringBuilder();

            foreach (var book in books)
            {
                output.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return output.ToString().TrimEnd();
        }

        // 8.Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder output = new StringBuilder();

            foreach (var author in authors)
            {
                output.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return output.ToString().TrimEnd();
        }

        // 9.Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();
            return String.Join(Environment.NewLine, books);
        }
    }
}
