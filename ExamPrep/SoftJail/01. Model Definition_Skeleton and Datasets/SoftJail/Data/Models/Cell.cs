using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(typeof(int),"1","1000")]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [InverseProperty(nameof(Prisoner.Cell))]
        public virtual ICollection<Prisoner> Prisoners { get; set; } = new List<Prisoner>();
    }

    /*
     *  Id – integer, Primary Key
        CellNumber – integer in the range [1, 1000] (required)
        HasWindow – bool (required)
        DepartmentId - integer, foreign key (required)
        Department – the cell's department (required)
        Prisoners - collection of type Prisoner
     */
}
