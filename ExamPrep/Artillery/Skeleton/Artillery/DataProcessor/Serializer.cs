
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Include(x => x.Guns)
                .ToArray()
                .Where(sh => sh.ShellWeight > shellWeight)
                .Select(sh => new
                {
                    sh.ShellWeight,
                    sh.Caliber,
                    Guns = sh.Guns
                        .Where(g => g.GunType.ToString() == "AntiAircraftGun")
                        .Select(g => new
                        {
                            GunType = g.GunType.ToString(),
                            g.GunWeight,
                            g.BarrelLength,
                            Range = g.Range > 3000 ? "Long-range" : "Regular range",
                        })
                        .OrderByDescending(g => g.GunWeight)
                        .ToArray(),
                })
                .OrderBy(sh => sh.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var guns = context.Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .Select(g => new ExportGunsDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    BarrelLength = g.BarrelLength,
                    GunWeight = g.GunWeight,
                    Range = g.Range,
                    Countries = g.CountriesGuns
                        .Where(cg => cg.Country.ArmySize > 4500000)
                        .Select(cg => new ExportGunsCountriesDto()
                        {
                            CountryName = cg.Country.CountryName,
                            ArmySize = cg.Country.ArmySize,
                        })
                        .OrderBy(cg => cg.ArmySize)
                        .ToArray(),
                })
                .OrderBy(g => g.BarrelLength)
                .ToArray();

            return XmlCoverter.XmlConverter.Serialize(guns, "Guns");
        }
    }
}
