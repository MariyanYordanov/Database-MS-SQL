using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeisterMask.Data.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^[A-Za-z0-9]+$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^(\d{3}-\d{3}-\d{4})$")]
        [MaxLength(12)]
        public string Phone { get; set; }

        [InverseProperty(nameof(EmployeeTask.Employee))]
        public ICollection<EmployeeTask> EmployeesTasks { get; set; } = new List<EmployeeTask>();
    }
    /*
     * Id - integer, Primary Key
     * 
     * Username - text with length [3, 40]. 
     * Should contain only lower or upper case letters and/or digits. (required)
     * 
     * Email – text (required). Validate it! There is attribute for this job.
     * 
     * Phone - text. Consists only of three groups (separated by '-'), 
     * the first two consist of three digits and the last one - of 4 digits. (required)
     * 
     * EmployeesTasks - collection of type EmployeeTask
     */
}
