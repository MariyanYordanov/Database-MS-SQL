﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^(The)\s([A-Z][a-z]*)$")]
        public string Nickname { get; set; }

        [Required]
        [Range(18,65)]
        public int Age { get; set; }

        [Required]
        public DateTime IncarcerationDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        [ForeignKey(nameof(Cell))]
        public int? CellId { get; set; }
        public Cell Cell { get; set; }

        [InverseProperty(nameof(Mail.Prisoner))]
        public virtual ICollection<Mail> Mails { get; set; } = new List<Mail>();

        [InverseProperty(nameof(OfficerPrisoner.Prisoner))]
        public virtual ICollection<OfficerPrisoner> PrisonerOfficers { get; set; } = new List<OfficerPrisoner>();
    }

    /*
     *  Id – integer, Primary Key
     *  
        FullName – text with min length 3 and max length 20 (required)

        Nickname – text starting with "The " and a single word only of letters with an uppercase letter for beginning (example:         ThePrisoner) (required)

        Age – integer in the range [18, 65] (required)

        IncarcerationDate – Date (required)

        ReleaseDate – Date

        Bail – decimal (non-negative, minimum value: 0)

        CellId – integer, foreign key

        Cell – the prisoner's cell

        Mails – collection of type Mail

        PrisonerOfficers - collection of type OfficerPrisoner*/
}