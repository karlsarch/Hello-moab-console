using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nymbl.Models.POCO
{
    public class ExerciseHint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 0)]
        public string Text { get; set; } = String.Empty;

        // Foreign Key
        public Guid ExerciseID { get; set; }
    }
}