using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab6.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        [SwaggerSchema(ReadOnly = true)]
        [Required]
        public Guid ID { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        [Display(Name = "Program")]
        public string Program { get; set; }
    }
}
