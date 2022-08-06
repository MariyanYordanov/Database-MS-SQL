using Artillery.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportGunsDto
    {
        [JsonProperty(nameof(ManufacturerId))]
        public int ManufacturerId { get; set; }

        [JsonProperty(nameof(GunWeight))]
        [Range(100, 1_350_000)]
        public int GunWeight { get; set; }

        [JsonProperty(nameof(BarrelLength))]
        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }

        [JsonProperty(nameof(NumberBuild))]
        public int? NumberBuild { get; set; }

        [JsonProperty(nameof(Range))]
        [Range(1, 100_000)]
        public int Range { get; set; }

        [JsonProperty(nameof(GunType))]
        public string GunType { get; set; }

        [JsonProperty(nameof(ShellId))]
        public int ShellId { get; set; }

        [JsonProperty(nameof(Countries))]
        public ImportGunsContriesDto[] Countries { get; set; }
    }
}
