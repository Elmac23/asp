using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lista5Zad1.Models
{
    public class TaskScoresViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Imiê jest wymagane")]
        [StringLength(100, ErrorMessage = "Imiê jest za d³ugie")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(100, ErrorMessage = "Nazwisko jest za d³ugie")]
        public string LastName { get; set; }

        [Required]
        public int[] Scores { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                yield return new ValidationResult("Imiê jest wymagane.", new[] { "FirstName" });
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                yield return new ValidationResult("Nazwisko jest wymagane.", new[] { "LastName" });
            }

            if (Scores == null)
            {
                yield return new ValidationResult("Scores are required.", new[] { "Scores" });
                yield break;
            }

            if (Scores.Length != 10)
            {
                yield return new ValidationResult("Exactly 10 scores are required.", new[] { "Scores" });
            }

            for (int i = 0; i < Scores.Length; i++)
            {
                int val = Scores[i];
                if (val < 0 || val > 100)
                {
                    yield return new ValidationResult($"Score for task {i + 1} must be between 0 and 100.", new[] { $"Scores[{i}]" });
                }
            }
        }
    }
}