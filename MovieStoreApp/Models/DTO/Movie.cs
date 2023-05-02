using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStoreApp.Models.BLL;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MovieStoreApp.Models.DTO
{
    public class Movie:IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Movie title must be between 5 dhe 50 caracters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Director is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Movie Director must be between 5 dhe 50 caracters")]
        public string Director { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Release year is required.")]
        [Range(1900, 2023, ErrorMessage = "Release year must be between 1900 and 2023.")]
        public int ReleaseYear { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Please select at least one category")]
        public List<CategoryViewModel> Categories { get; set; }


       


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var specialChars = "!@#$%^&*()_+<>?/.,;:'\"[]{}\\|`~";
            if (Title.Any(c => specialChars.Contains(c)))
            {
                yield return new ValidationResult("Movie title can not contain special caracters ", new string[] { nameof(Title) });
            }
        }

        
    }
}
