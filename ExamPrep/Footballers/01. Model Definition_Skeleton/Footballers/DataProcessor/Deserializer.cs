namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using XmlConverter;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static object ImportCoachesDto { get; private set; }
        public static DateTimeStyles Cultureinfo { get; private set; }

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Coach> coaches = new List<Coach>();

            var dtos = XmlConverter.Deserializer<ImportCoahesDto>(xmlString, "Coaches");

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dto.Nationality == null || dto.Nationality == string.Empty)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                     Name = dto.Name,
                      Nationality = dto.Nationality,
                };

                foreach (var fDto in dto.Footballers)
                {
                    if (!IsValid(fDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(fDto.ContractStartDate, 
                        "dd/MM/yyyy", CultureInfo.InvariantCulture, 
                        DateTimeStyles.None,out DateTime validStartDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(fDto.ContractEndDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime validEndDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (validStartDate > validEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractStartDate = validStartDate,
                        ContractEndDate = validEndDate,
                        BestSkillType = (BestSkillType)fDto.BestSkillType,
                        PositionType = (PositionType)fDto.PositionType
                    };

                    coach.Footballers.Add(footballer);
                }

                coaches.Add(coach);
                sb.AppendLine(String.Format(SuccessfullyImportedCoach,coach.Name, coach.Footballers.Count()));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();  
            List<Team> teams = new List<Team>();

            var dtos = JsonConvert.DeserializeObject<List<ImportTeamsDto>>(jsonString);

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = dto.Name,
                    Nationality = dto.Nationality,
                    Trophies = dto.Trophies,
                };

                // take unique footballer
                foreach (int id in dto.Footballers.Distinct()) 
                {
                    // search unique footballer in database
                    if (context.Footballers.Find(id) == null) 
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer teamFootballer = new TeamFootballer()
                    {
                        Team = team,
                        FootballerId = id,
                    };

                    team.TeamsFootballers.Add(teamFootballer);
                }

                teams.Add(team);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count()));
            }

            context.Teams.AddRange(teams);
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
