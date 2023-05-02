using System.ComponentModel.DataAnnotations;

namespace MovieStoreApp.Models.DTO
{
    public class Category: IValidatableObject
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a name for the category.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The category name must be between 3 and 50 characters long.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "The description cannot be longer than 500 characters.")]
        public string Description { get; set; }
        public List<MovieCategory> CategoriesWithMovies { get; set; } = new List<MovieCategory>();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var specialChars = "!@#$%^&*()_+<>?/.,;:'\"[]{}\\|`~";
            if (Name.Any(c => specialChars.Contains(c)))
            {
                yield return new ValidationResult("Name can not contain special chars. ", new string[] { nameof(Name) });
            }
        }

    }
}
