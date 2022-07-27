namespace BookShop
{
    using Data;
    using System;
    using Initializer;
    using System.Linq;
    using System.Text;
    using BookShop.Models.Enums;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            int input = int.Parse(Console.ReadLine());

            Console.WriteLine(GetBooksNotReleasedIn(context, input));
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
    }
}
