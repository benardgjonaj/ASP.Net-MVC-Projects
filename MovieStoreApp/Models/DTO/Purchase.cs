using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MovieStoreApp.Models.DTO
{
    public class Purchase
    {
        public int Id { get; set; }
       
        public List<Movie> Movies { get;set; }

        
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Purchase Description")]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
