namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using BookShop.XmlCoverter;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var dtos = XmlConverter.Deserializer<ImportBooksDto>(xmlString, "Books");
            List<Book> books = new List<Book>();
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                if (!DateTime.TryParseExact(dto.PublishedOn, "MM/dd/yyyy", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime validDate))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                Book book = new Book()
                {
                    Name = dto.Name,
                    Genre = (Genre)dto.Genre,
                    Price = dto.Price,
                    Pages = dto.Pages,
                    PublishedOn = validDate,
                };

                books.Add(book);
                stringBuilder.AppendLine(String.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var dtos = JsonConvert.DeserializeObject<ImportAuthorsDto[]>(jsonString);
            List<Author> authorList = new List<Author>();
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (authorList.FirstOrDefault(a => a.Email == dto.Email) != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Author author = new Author()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Phone = dto.Phone,
                    Email = dto.Email,
                };

                foreach (var bDto in dto.Books)
                {
                    Book book = context.Books.Find(bDto.Id);

                    if (book == null)
                    {
                        continue;
                    }

                    author.AuthorsBooks.Add(new AuthorBook()
                    {
                        Author = author,
                        Book = book,
                    });
                }

                if (!author.AuthorsBooks.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authorList.Add(author);
                sb.AppendLine(String.Format(SuccessfullyImportedAuthor, $"{author.FirstName} {author.LastName}", author.AuthorsBooks.Count()));
            }

            context.Authors.AddRange(authorList);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}