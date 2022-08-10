using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{

    [XmlType("Task")]
    public class ImportProjectsTasksDto
    {
        [Required]
        [XmlElement]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }

        [Required]
        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [Required]
        [XmlElement(nameof(ExecutionType))]
        [EnumDataType(typeof(ExecutionType))]
        public int ExecutionType { get; set; }

        [Required]
        [XmlElement(nameof(LabelType))]
        [EnumDataType(typeof(LabelType))]
        public int LabelType { get; set; }
    }
}
