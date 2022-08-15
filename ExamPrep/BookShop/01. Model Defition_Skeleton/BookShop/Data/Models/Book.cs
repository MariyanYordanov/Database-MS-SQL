using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Data.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; } 

        [Range(typeof(decimal),"0.01", "79228162514264337593543950335")]
        [StringLength(29, MinimumLength = 4)]
        public decimal Price { get; set; }

        [Range(50,5_000)]
        public int Pages { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        [InverseProperty(nameof(AuthorBook.Book))]
        public ICollection<AuthorBook> AuthorsBooks { get; set; } = new HashSet<AuthorBook>();
    }
}
