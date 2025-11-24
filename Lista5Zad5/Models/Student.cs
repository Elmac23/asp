using System.ComponentModel.DataAnnotations;

namespace Lista5Zad5.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Range(17, 100)]
        public int Age { get; set; }
    }
}