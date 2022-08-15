namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.XmlConverter;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Despatcher> despathers = new List<Despatcher>();

            var dtos = XmlConverter.Deserializer<ImportDespatchersDto>(xmlString, "Despatchers");

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (String.IsNullOrEmpty(dto.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = dto.Name,
                    Position = dto.Position,
                };

                foreach (ImportDespatcherTrucksDto tDto in dto.Trucks)
                {
                    if (!IsValid(tDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck()
                    {
                        RegistrationNumber = tDto.RegistrationNumber,
                        VinNumber = tDto.VinNumber,
                        TankCapacity = tDto.TankCapacity,
                        CargoCapacity = tDto.CargoCapacity,
                        CategoryType = (CategoryType)tDto.CategoryType,
                        MakeType = (MakeType)tDto.MakeType,
                    };

                    despatcher.Trucks.Add(truck);
                }

                despathers.Add(despatcher);
                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count()));
            }

            context.Despatchers.AddRange(despathers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Client> clients = new List<Client>();
            var dtos = JsonConvert.DeserializeObject<ImportClientsDto[]>(jsonString);
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (String.IsNullOrEmpty(dto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = dto.Name,
                    Nationality = dto.Nationality,
                    Type = dto.Type,
                };

                foreach (var tDto in dto.Trucks.Distinct())
                {
                    Truck truck = context.Trucks.Find(tDto);

                    if (truck == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck = new ClientTruck()
                    {
                        Client = client,
                        Truck = truck,
                    };

                    client.ClientsTrucks.Add(clientTruck);

                   
                }
                 clients.Add(client);
                    sb.AppendLine(String.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count()));

            }

            context.Clients.AddRange(clients);
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
