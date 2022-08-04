namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using XmlConverter;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            PlaysDto[] playsDto = XmlConverter.Deserializer<PlaysDto>(xmlString, "Plays");
            List<Play> plays = new List<Play>();
            foreach (var pDto in playsDto)
            {
                if (!IsValid(pDto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan duration = TimeSpan.ParseExact(pDto.Duration,"c",CultureInfo.InvariantCulture);
                if (duration.Hours < 1)
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                if (!Enum.TryParse(typeof(Genre), pDto.Genre, out var genre))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                plays.Add(new Play
                {
                    Title = pDto.Title,
                    Duration = duration,
                    Rating = pDto.Rating,
                    Genre = (Genre)genre,
                    Description = pDto.Description,
                    Screenwriter = pDto.Screenwriter,
                });

                stringBuilder.AppendLine(String.Format(SuccessfulImportPlay, pDto.Title, pDto.Genre, pDto.Rating));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            CastsDto[] castsDto = XmlConverter.Deserializer<CastsDto>(xmlString, "Casts");
            List<Cast> casts = new List<Cast>();
            foreach (CastsDto cDto in castsDto)
            {
                if (!IsValid(cDto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                casts.Add(new Cast()
                {
                    FullName = cDto.FullName,
                    IsMainCharacter = cDto.IsMainCharacter,
                    PhoneNumber = cDto.PhoneNumber,
                    PlayId = cDto.PlayId,
                });

                stringBuilder.AppendLine(String
                    .Format(SuccessfulImportActor, cDto.FullName, cDto.IsMainCharacter ? "main" : "lesser"));
            }
            context.Casts.AddRange(casts);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<Theatre> validTheaters = new List<Theatre>();

            TheatersTicketsDto[] theaters = JsonConvert.DeserializeObject<TheatersTicketsDto[]>(jsonString);

            foreach (TheatersTicketsDto theatre in theaters)
            {
                if (!IsValid(theatre))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre validTheatre = new Theatre()
                {
                     Name = theatre.Name,
                     NumberOfHalls = theatre.NumberOfHalls,
                     Director = theatre.Director,
                };

                foreach (TicketsDto ticket in theatre.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    validTheatre.Tickets.Add(new Ticket()
                    {
                        Price = ticket.Price,
                        PlayId = ticket.PlayId,
                        RowNumber = ticket.RowNumber,
                    });
                }

                validTheaters.Add(validTheatre);

                stringBuilder.AppendLine(String.
                    Format(SuccessfulImportTheatre, validTheatre.Name, validTheatre.Tickets.Count));
            }

            context.Theatres.AddRange(validTheaters);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
