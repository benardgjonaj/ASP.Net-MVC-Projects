using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MovieStoreApp.Models;
using MovieStoreApp.Models.BLL;
using MovieStoreApp.Models.DTO;
using MovieStoreApp.Models.Enums;
using System.Reflection;


namespace MovieStoreApp.Controllers
{
    public class MovieController : BaseController
    {
        string rootPath = "";
        public MovieController(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            rootPath = _environment.WebRootPath;
        }
        private static List<Movie> _movie = new List<Movie>();

        public IActionResult Index()
        {
            return View(MovieManager.GetMovies());
   
        }
        [HttpGet]
        public IActionResult Create()
        {
            if(!IsAdmin)
                return this.RedirectToHomePage();

            var categories =CategoriesManager.GetCategoriesViewModel();

            
            var model = new Movie
            {
                Categories = new List<CategoryViewModel>()
            };

            foreach (var category in categories)
            {
                var categoryViewModel = new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsSelected = false
                };

                model.Categories.Add(categoryViewModel);
            }

            return View(model);

        }
        [HttpPost]
        public IActionResult Create(Movie movie)
         {
            if (AuthorizedUser==null || AuthorizedUser.PersonType==PersonType.USER)
                return this.RedirectToHomePage();
            try
            {
                if (ModelState.IsValid)
                {


                    var insertedMovie = MovieManager.AddNewMovie(movie);
                    if (insertedMovie != null && insertedMovie.Id > 0)
                    {
                        EditMovieImage(insertedMovie.Id);
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", $"{ex.Message}");
            }

            
            
                return View(movie);
            
            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin)
                return this.RedirectToHomePage();

            Movie movieToEdit=MovieManager.GetMovieById(id);
            var selectedcategories=CategoriesManager.GetCategoriesByMovieId(id);
            foreach (var category in movieToEdit.Categories)
            {
                foreach (var item in selectedcategories)
                {
                    if (item.Id == category.Id)
                    {
                        category.IsSelected=true;
                    }

                }
            }
            if (movieToEdit != null)
            {
                return View(movieToEdit);
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult Edit(int id, Movie model)
     {
            if (!IsAdmin)
                return this.RedirectToHomePage();
            if (ModelState.IsValid)
            {
                try
                {
                    var editedModel = MovieManager.EditMovie(id, model);
                    if (editedModel != null)
                    {
                        EditMovieImage(editedModel.Id);
                        return RedirectToAction("Index");

                    }


                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"{ex.Message}");
                    ModelState.AddModelError("", "data did not saved");
                }
                
            }
            
            
                return View(model);
            
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Movie movieDetails = MovieManager.GetMovieById(id);
            var selectedcategories = CategoriesManager.GetCategoriesByMovieId(id);
           movieDetails.Categories = selectedcategories;
            if (movieDetails != null)
            {
                return View(movieDetails);
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin)
                return this.RedirectToHomePage();
            var movie=MovieManager.GetMovieById(id);
          
            if(movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            if (!IsAdmin)
                return this.RedirectToHomePage();
            var movie= MovieManager.GetMovieById(id);
            if( movie == null)
            {
                return NotFound();
            }
            MovieManager.DeleteMovie(id);
            return RedirectToAction("Index");
        }
        private void EditMovieImage(int movieId)
        {
            try
            {
                var hasFiles = this.HttpContext.Request.Form.Files.Any();
                if (hasFiles)
                {
                    var file = this.HttpContext.Request.Form.Files[0];
                    var fileName = file.FileName;
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = $"{movieId}{fileExtension}";
                    var filePath = Path.Combine(rootPath, "Images", "Movies", newFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    MovieManager.SetImagePath(movieId, newFileName);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}