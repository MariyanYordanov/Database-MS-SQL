﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            this.StudentsEnrolled = new HashSet<StudentCourse>();
            this.Resources = new HashSet<Resource>();
            this.HomeworkSubmissions = new HashSet<Homework>();
        }

        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [InverseProperty(nameof(StudentCourse.Course))]
        public virtual ICollection<StudentCourse> StudentsEnrolled { get; set; }

        [InverseProperty(nameof(Resource.Course))]
        public virtual ICollection<Resource> Resources { get; set; }

        [InverseProperty(nameof(Homework.Course))]
        public virtual ICollection<Homework> HomeworkSubmissions { get; set; }
    }
}
