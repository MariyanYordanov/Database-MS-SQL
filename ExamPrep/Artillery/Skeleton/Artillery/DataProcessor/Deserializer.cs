namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var dtos = XmlCoverter.XmlConverter.Deserializer<ImportCountriesDto>(xmlString, "Countries");

            List<Country> countries = new List<Country>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = dto.CountryName,
                    ArmySize = dto.ArmySize,
                };

                countries.Add(country);
                stringBuilder.AppendLine(String.Format(SuccessfulImportCountry,country.CountryName,country.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var dtos = XmlCoverter.XmlConverter.Deserializer<ImportManufacturersDto>(xmlString, "Manufacturers");

            List<Manufacturer> manufacturers = new List<Manufacturer>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                if (manufacturers.Any(x => x.ManufacturerName == dto.ManufacturerName))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                     ManufacturerName = dto.ManufacturerName,
                     Founded = String.Join(", ",dto.Founded.Split(", ").Skip(1).TakeLast(2)),
                };

                manufacturers.Add(manufacturer);
                stringBuilder.AppendLine(
                    String.Format(SuccessfulImportManufacturer,manufacturer.ManufacturerName, manufacturer.Founded));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var dtos = XmlCoverter.XmlConverter.Deserializer<ImportShellsDto>(xmlString, "Shells");

            List<Shell> shells = new List<Shell>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = dto.ShellWeight,
                    Caliber = dto.Caliber,
                };

                shells.Add(shell);
                stringBuilder.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<Gun> guns = new List<Gun>();
            
            var dtos = JsonConvert.DeserializeObject<ImportGunsDto[]>(jsonString);

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    if (!IsValid(dto))
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }
                }

                if (!Enum.TryParse(dto.GunType, out GunType gunType))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = dto.ManufacturerId,
                    GunWeight = dto.GunWeight,
                    BarrelLength = dto.BarrelLength,
                    NumberBuild = dto.NumberBuild,
                    Range = dto.Range,
                    GunType = gunType,
                    ShellId = dto.ShellId,
                };

                foreach (var cDto in dto.Countries)
                {
                    CountryGun countryGun = new CountryGun()
                    {
                        Gun = gun,
                        CountryId = cDto.Id,
                    };

                    gun.CountriesGuns.Add(countryGun);
                }

                guns.Add(gun);
                stringBuilder.AppendLine(String.
                    Format(SuccessfulImportGun,gun.GunType, gun.GunWeight, gun.BarrelLength));
            }

            context.Guns.AddRange(guns);
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
