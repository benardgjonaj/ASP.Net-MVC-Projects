using MovieStoreApp.Models.DAL;

namespace MovieStoreApp.Models.BLL
{
    public class MovieCategoriesManager
    {
        public static List<DTO.Movie> GetMoviesByCategoryId(int categoryId)
        {
            var movies = MovieCategory.GetMoviesByCategoryId(categoryId);
            var dtoMovies = new List<DTO.Movie>();
            foreach (var movie in movies)
            {
                var dtomovie = new DTO.Movie()
                {
                    Title = movie.Title,
                    Description = movie.Description,
                    ReleaseYear = movie.ReleaseYear,
                    Price = movie.Price,
                };
                dtoMovies.Add(dtomovie);
            }
            return dtoMovies;
        }
        public static List<DTO.CategoryViewModel> GetCategoriesByMovieId(int movieId) {
            var categories = Category.GetCategoriesByMovieId(movieId);
            var dtoCategories = new List<DTO.CategoryViewModel>();
            foreach (var category in categories)
            {
                var dtoCategory = new DTO.CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name
                };
                dtoCategories.Add(dtoCategory);
            }
            return dtoCategories;
        }


    }
}
