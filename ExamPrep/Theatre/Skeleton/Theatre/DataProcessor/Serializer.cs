namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                .ToArray()
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count() >= 20)
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = Decimal.Parse(t.Tickets
                        .Where(x => x.RowNumber > 0 && x.RowNumber < 6)
                        .Sum(t => t.Price).ToString("f2")),
                    Tickets = t.Tickets
                        .Where(x => x.RowNumber > 0 && x.RowNumber < 6)
                        .Select(x => new
                        {
                            x.Price,
                            x.RowNumber
                        })
                        .OrderByDescending(x => x.Price)
                        .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context.Plays
                .ToArray()
                .Where(p => p.Rating <= rating)
                .Select(p => new PlaysExportDto
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = Decimal.Parse(p.Rating.ToString("f1")) == 0 ? "Premier" : $"{p.Rating:f1}",
                    Genre = p.Genre,
                    Actors = p.Casts
                        .Where(c => c.IsMainCharacter)
                        .Select(x => new ActorDto
                        {
                            FullName = x.FullName,
                            MainCharacter = $"Plays main character in '{p.Title}'." ,
                        })
                        .OrderByDescending(p => p.FullName)
                        .ToArray(),
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            return XmlConverter.XmlConverter.Serialize<PlaysExportDto>(plays, "Plays");
        }
    }
}
