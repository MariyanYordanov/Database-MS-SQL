namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using BookShop.XmlCoverter;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new 
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    Books = a.AuthorsBooks
                        .Select(ab => new 
                        {
                            BookName = ab.Book.Name,
                            BookPrice = ab.Book.Price.ToString("f2"),
                        })
                        .OrderByDescending(ab => Decimal.Parse(ab.BookPrice))
                        .ToArray(),
                })
                .OrderByDescending(a => a.Books.Count())
                .ThenBy(a => a.AuthorName)
                .ToArray();

            return JsonConvert.SerializeObject(authors, Formatting.Indented);
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var oldestBooks = context.Books
                .Where(b => b.PublishedOn < date && b.Genre == Genre.Science)
                .Select(b => new ExportOldestBooksDto()
                {
                    Name = b.Name,
                    Pages = b.Pages,
                    Date = b.PublishedOn.ToString("d",CultureInfo.InvariantCulture),
                })
                .OrderByDescending(b => b.Pages)
                .ThenByDescending(b => b.Date)
                .Take(10)
                .ToArray();

            return XmlConverter.Serialize(oldestBooks,"Books");
        }
    }
}