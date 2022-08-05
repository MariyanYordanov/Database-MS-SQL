using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public Department()
        {
            this.Cells = new HashSet<Cell>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        [InverseProperty(nameof(Cell.Department))]
        public virtual ICollection<Cell> Cells { get; set; } 
    }

    /*
     *  Id – integer, Primary Key
        Name – text with min length 3 and max length 25 (required)
        Cells - collection of type Cell
     */
}
