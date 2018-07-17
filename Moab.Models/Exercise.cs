using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nymbl.Models.POCO
{
    public class Exercise
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Display(Description = "Name of the Exercise")]
        [StringLength(65, ErrorMessage = "The Exercise Name value cannot exceed 65 characters. ")]
        public string Name { get; set; } = string.Empty;

        [Display(Description = "True indicates that this exercise is based on the number of repetitions. False indicates that the exercise is duration based.")]
        public bool HasRepetitionTarget { get; set; }

        [Display(Description = "The code for this Exercise that is used in the App. Examples STAB_001; STR_003")]
        public string ExerciseCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        public ICollection<ExerciseHint> ExerciseHints { get; set; } = new HashSet<ExerciseHint>();

        /// <summary>
        /// Optional text to indicate how a user can make the exercise less difficult.
        /// </summary>
        public string EasierHint { get; set; }

        /// <summary>
        /// Optional text to indicate how a user can make the exercise more difficult.
        /// </summary>
        public string HarderHint { get; set; }
    }
}