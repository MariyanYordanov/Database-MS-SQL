using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TeisterMask.Data.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }

        public DateTime? DueDate { get; set; }

        [InverseProperty(nameof(Task.Project))]
        public ICollection<Task> Tasks { get; set; } 
            = new List<Task>();
    }

    /*
     * Id - integer, Primary Key
     * Name - text with length [2, 40] (required)
     * OpenDate - date and time (required)
     * DueDate - date and time (can be null)
     * Tasks - collection of type Task
     */
}
