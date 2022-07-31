using CarDealer.Data;
using System.IO;
using System;
using System.Xml.Serialization;
using CarDealer.Dtos.Import;
using System.Linq;
using CarDealer.Models;
using CarDealer.XmlHelper;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext carDealerContext = new CarDealerContext();
            string xml = File.ReadAllText("../../../Datasets/parts.xml");

            string result = ImportParts(carDealerContext, xml);
            Console.WriteLine(result);
            //carDealerContext.Database.EnsureDeleted();
            //carDealerContext.Database.EnsureCreated();
        }

        // Query 9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            const string root = "Suppliers";

            var suppliersDto = XmlConverter.Deserializer<SupplierDto>(inputXml, root);

            Supplier[] suppliers = suppliersDto
                .Select(dto => new Supplier
                {
                    Name = dto.Name,
                    IsImporter = dto.IsImporter,
                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        // Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            const string root = "Parts";

            var partsDtos = XmlConverter.Deserializer<PartsDto>(inputXml, root);

            var validId = context.Suppliers.Select(s => s.Id).ToArray();

            var parts = partsDtos
                .Where(p => validId.Contains(p.SupplierId))
                .Select(dto => new Part
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId,
                })
               .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }


    }
}