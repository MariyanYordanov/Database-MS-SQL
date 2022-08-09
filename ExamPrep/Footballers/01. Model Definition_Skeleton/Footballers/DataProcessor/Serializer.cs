namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.Linq;
    using XmlConverter;

    public class Serializer
    {
        public static IFormatProvider Cultureinfo { get; private set; }

        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Where(c => c.Footballers.Count() > 0)
                .ToArray()
                .Select(c => new ExportCoachesDto()
                {
                    CoachName = c.Name,
                    FootballersCount = c.Footballers.Count(),
                    Footballers = c.Footballers
                        .Select(f => new ExportCoachFootballersDto()
                        {
                            Name = f.Name,
                            Position = f.PositionType.ToString(),
                        })
                        .OrderBy(f => f.Name)
                        .ToArray(),
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.CoachName)
                .ToArray();

            return XmlConverter.Serialize(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {

            var teams = context.Teams
               .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
               .ToArray()
               .Select(t => new
               {
                   t.Name,
                   Footballers = t.TeamsFootballers
                    .Where(f => f.Footballer.ContractStartDate >= date)
                    .OrderByDescending(f => f.Footballer.ContractEndDate)
                    .ThenBy(f => f.Footballer.Name)
                    .Select(f => new
                    {
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate
                            .ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate
                            .ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.Footballer.BestSkillType.ToString(),
                        PositionType = f.Footballer.PositionType.ToString(),
                    })
                    .ToArray(),
               })
               .OrderByDescending(f => f.Footballers.Count())
               .ThenBy(f => f.Name)
               .Take(5)
               .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
