using System.ComponentModel.DataAnnotations;

namespace PetsRegistration.Api.Dtos
{
    public class UpdatePetDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, 30, ErrorMessage = "Age must be a valid integer between 0 and 30.")]
        public int Age { get; set; }

        [RegularExpression("^(male|female)$", ErrorMessage = "Sex must be either 'male' or 'female'.")]
        public string Sex { get; set; }

        [Required]
        [Range(0.1, 100.0, ErrorMessage = "Weight must be a valid number between 0.1 and 100.")]
        public double Weight { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        [EmailAddress]
        public string OwnerEmail { get; set; }
    }
}