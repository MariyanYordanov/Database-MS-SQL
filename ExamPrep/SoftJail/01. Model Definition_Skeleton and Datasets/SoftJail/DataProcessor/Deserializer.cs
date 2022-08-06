namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using XmlConverter;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<Department> departments = new List<Department>();

            DepartmentsCellsImportDto[] dtos = JsonConvert.DeserializeObject<DepartmentsCellsImportDto[]>(jsonString);

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }

                if (dto.Cells.Count() == 0)
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }

                if (dto.Cells.Any(c => !IsValid(c)))
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }


                Department department = new Department()
                {
                    Name = dto.Name,
                };


                foreach (var cell in dto.Cells)
                {
                    Cell currCell = new Cell()
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow,
                    };

                    department.Cells.Add(currCell);
                }

                departments.Add(department);
                stringBuilder.AppendLine($"Imported {department.Name} with {department.Cells.Count()} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<Prisoner> prisoners = new List<Prisoner>();
            PrisonersMailsImportDto[] dtos = JsonConvert.DeserializeObject<PrisonersMailsImportDto[]>(jsonString);

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }

                if (dto.Mails.Any(m => !IsValid(m)))
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }


                bool isIncarcerationDate =
                    DateTime.TryParseExact(dto.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime incarcerationDate);
                if (!isIncarcerationDate)
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releseDate = null;
                if (!String.IsNullOrEmpty(dto.ReleaseDate))
                {
                    bool isReleseDate = 
                        DateTime.TryParseExact(dto.ReleaseDate, "dd/MM/yyyy", 
                        CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out DateTime releseDateValue);
                    if (!isReleseDate)
                    {
                        stringBuilder.AppendLine("Invalid Data");
                        continue;
                    }

                    releseDate = releseDateValue;
                }
                
                Prisoner prisoner = new Prisoner()
                {
                    FullName = dto.FullName,
                    Age = dto.Age,
                    Bail = dto.Bail,
                    CellId = dto.CellId,
                    Nickname = dto.Nickname,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releseDate,
                };

                foreach (var mail in dto.Mails)
                {
                    Mail currMail = new Mail()
                    {
                        Address = mail.Address,
                        Description = mail.Description,
                        Sender = mail.Sender,
                    };

                    prisoner.Mails.Add(currMail);
                }

                prisoners.Add(prisoner);
                stringBuilder.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            ImportOfficersPrisonersDto[] dtos = XmlConverter
                .Deserializer<ImportOfficersPrisonersDto>(xmlString, "Officers");

            var officers = new List<Officer>();

            foreach (ImportOfficersPrisonersDto dto in dtos)
            {
                if (!IsValid(dto))
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }

                if (!Enum.TryParse(dto.Position, out Position position))
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }

                if (!Enum.TryParse(dto.Weapon, out Weapon weapon))
                {
                    stringBuilder.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = dto.FullName,
                    Salary = dto.Salary,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = dto.DepartmentId
                };

                foreach (var pDto in dto.Prisoners)
                {
                    OfficerPrisoner prisoner = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = pDto.Id
                    };

                    officer.OfficerPrisoners.Add(prisoner);
                }

                officers.Add(officer);
                stringBuilder.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count()} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}