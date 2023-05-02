using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStoreApp.Models.DAL;
using System.Data.SqlClient;

namespace MovieStoreApp.Models.BLL
{
    public class MovieManager
    {
      
        public static List<DTO.Movie> GetMovies() {
            List<Movie> dbmovies = Movie.GetMovies();
            List<DTO.Movie>result = new List<DTO.Movie>();
            foreach(var movie in dbmovies)
            {
                result.Add(new DTO.Movie
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Director= movie.Director,
                    Description = movie.Description,
                    Price = movie.Price,
                    ReleaseYear = movie.ReleaseYear,
                    Image=movie.Image,
                    

                });
            }
            return result;

        }
        public static DTO.Movie GetMovieById(int id)
        {
            Movie dbMovie = Movie.GetMovieById(id);
            if (dbMovie != null)
            {
                var selectedCategories = MovieCategory.GetSelectedCategoryIds(id);
                var allCategories = CategoriesManager.GetCategoriesViewModel();

                return new DTO.Movie
                {
                    Title = dbMovie.Title,
                    Director = dbMovie.Director,
                    Description = dbMovie.Description,
                    Price = dbMovie.Price,
                    ReleaseYear = dbMovie.ReleaseYear,
                    Id = dbMovie.Id,
                    Image= dbMovie.Image,
                    Categories = allCategories
                };
            }
            return null;
        }
        public static DTO.Movie AddNewMovie(DTO.Movie model)
        {
            var existsMovieByTitle = DAL.Movie.GetByTitle(model.Title);
            if (existsMovieByTitle != null)
            {
                throw new Exception("Already exist a movie by this title: ");
                return null;
            }
            DAL.Movie movieToInsert = new DAL.Movie
            {
                Title = model.Title,
                Director = model.Director,
                Description = model.Description,
                Price = model.Price,
                ReleaseYear = model.ReleaseYear,
                
               
            };
             
            int resultId = Movie.Insert(movieToInsert); 
            
            if (resultId>0)
            {
                var catIds = new List<int>();
                bool added;
                foreach (var item in model.Categories)
                {
                    if (item.IsSelected)
                    {
                        //added = MovieCategory.Insert(new MovieCategory { MovieId = resultId, CategoryId = item.Id });
                        //if (added == false)
                        //    return null;
                        catIds.Add(item.Id);

                    }
                }
               added= MovieCategory.Insert(new MovieCategory { MovieId=resultId,CategoryIds=catIds});
                if(added==false) { return null; }
                return GetMovieById(resultId);
            }
            
            else
            {
                return null;
            }

        }
        public static DTO.Movie EditMovie(int id, DTO.Movie model)
        {
            
            Movie movie =Movie.GetMovieById(id);
            
            if (movie != null)
            {
               var existsMovieByTitle = DAL.Movie.GetByTitle(model.Title);
                if (existsMovieByTitle != null && existsMovieByTitle.Id != id)
                {
                    throw new Exception("Already exist a movie by this title: ");
                }
                movie.Title = model.Title;
                movie.Director = model.Director;
                movie.Description = model.Description;
                movie.ReleaseYear = model.ReleaseYear;
                movie.Price = model.Price;
                movie.Image=movie.Image;
               
                bool updated = Movie.Update(movie);
                List<int> selectedCategories = new List<int>();
                foreach (var item in model.Categories)
                {
                    if (item.IsSelected)
                    {
                        selectedCategories.Add(item.Id);
                    }
                }
                if (updated)
                {
                    var moviecategory = new MovieCategory()
                    {
                        MovieId = id,
                        CategoryIds = selectedCategories

                    };
                   if( MovieCategory.Update(moviecategory))
                   
                    return GetMovieById(id);
                    
                }
            }

           
            return null;
        }
        
        public static void DeleteMovie(int id)
        {
            bool resul=Movie.DeleteMovie(id);
            if (resul)
                MovieCategory.Delete(id);
            
        }
        public static List<DTO.Movie> GetMoviesBypurchaseId(int id)
        {
            List<DAL.Movie> moviesDAl = DAL.Movie.GetMoviesBypurchaseId(id);
            List<DTO.Movie> moviesDTo = new List<DTO.Movie>();
            foreach (DAL.Movie movie in moviesDAl)
            {
                DTO.Movie dtoMovie = new DTO.Movie
                {
                    Title = movie.Title,
                    Description = movie.Description,
                    Director = movie.Director,
                    ReleaseYear = movie.ReleaseYear,
                    Price= movie.Price,
                    Id = movie.Id,
                   
                };
                moviesDTo.Add(dtoMovie);
            }
            return moviesDTo;
        }
        internal static void SetImagePath(int id, string newFileName)
        {
            DAL.Movie.SetImagePath(id, newFileName);
        }
        public static List<DTO.Movie> GetTop6FeaturedMovies()
        {
            List<Movie> dbmovies = Movie.GetTop6FeaturedMovies();
            List<DTO.Movie> result = new List<DTO.Movie>();
            foreach (var movie in dbmovies)
            {
                result.Add(new DTO.Movie
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Director = movie.Director,
                    Description = movie.Description,
                    Price = movie.Price,
                    ReleaseYear = movie.ReleaseYear,
                    Image = movie.Image,


                });
            }
            return result;

        }
        public static List<DTO.Movie> SearchMovies(string keywords)
        {
            List<Movie> moviesdal = DAL.Movie.SearchMovies(keywords);
            List<int> catogoryids=Category.SearchCategories(keywords);
            foreach (int item in catogoryids)
            {
               var catmovies= Movie.GetMoviesByCategoryId(item);
                foreach (var movie in catmovies)
                {
                    moviesdal.Add(movie);
                }
            }
            List<DTO.Movie> moviesdto = new List<DTO.Movie>();
            foreach (var movie in moviesdal)
            {
                DTO.Movie moviedto = new DTO.Movie
                {
                    Id= movie.Id,
                    Title = movie.Title,
                    Director = movie.Director,
                    Description = movie.Description,
                    Price = movie.Price,
                    ReleaseYear = movie.ReleaseYear,
                    Image= movie.Image,
                };
                moviesdto.Add(moviedto);

            }
           
            return moviesdto;
           
        }


    }
}
