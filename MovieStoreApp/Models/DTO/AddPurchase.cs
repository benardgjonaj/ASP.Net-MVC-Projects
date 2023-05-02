using Microsoft.Build.Framework;
using MovieStoreApp.Models.BLL;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace MovieStoreApp.Models.DTO
{
    public class AddPurchase
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select at least one movie")]
        public List<int> SelectedMovieIds { get; set; }

        public List<MovieModel> Movies
        {
            get
            {
                List<MovieModel> movies = new List<MovieModel>();
                List<Movie> allMovies = MovieManager.GetMovies();

                foreach (Movie movie in allMovies)
                {
                    MovieModel moviemodel = new MovieModel();
                    moviemodel.Id = movie.Id;
                    moviemodel.Title = movie.Title;

                    movies.Add(moviemodel);
                }

                return movies;
            }
            set { }
        }
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Purchase Description")]
        public string Description { get; set; }
        
    }
}
