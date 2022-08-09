using Footballers.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportCoachFootballersDto
    {
        [XmlElement(nameof(Name))]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [XmlElement(nameof(ContractStartDate))]
        public string ContractStartDate { get; set; }

        [XmlElement(nameof(ContractEndDate))]
        public string ContractEndDate { get; set; }

        [XmlElement(nameof(PositionType))]
        [EnumDataType(typeof(PositionType))]
        public int PositionType { get; set; }

        [XmlElement(nameof(BestSkillType))]
        [EnumDataType(typeof(BestSkillType))]
        public int BestSkillType { get; set; }
    }
}
