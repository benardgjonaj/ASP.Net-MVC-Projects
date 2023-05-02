using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using MovieStoreApp.Models.BLL;

using MovieStoreApp.Models.DTO;
using MovieStoreApp.Models.Enums;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MovieStoreApp.Controllers
{
    public class UsersController : BaseController
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View(new Person());
            if (AuthorizedUser == null || AuthorizedUser.PersonType == PersonType.ADMIN)
            {
                return View(new Person());
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(Person person) {
            if (ModelState.IsValid)
            {
                if (PersonsMenager.Register(person))
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    ModelState.AddModelError("", "Emaili ekziston");
                }
            }
            return View(person);
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (!IsAuthorized)
            {
                return View(new PersonLoginModel());
            }
            return this.RedirectToHomePage();
        }
        [HttpPost]
        public IActionResult Login(PersonLoginModel model)
        {
            if (ModelState.IsValid)
            {
                //save to database
                var person = PersonsMenager.Login(model.Email, model.Password);
                if (person != null)
                {
                    HttpContext.Session.SetString("email", person.Email);
                    HttpContext.Session.SetInt32("id", person.Id);
                    HttpContext.Session.SetInt32("personType", (int)person.PersonType);
                    return this.RedirectToHomePage(person);
                }
                else
                {
                    ModelState.AddModelError("", "Emaili ose passwordi eshte gabim");
                }

            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult PurchaseMovie()
        {
            if (AuthorizedUser == null || AuthorizedUser.PersonType == PersonType.ADMIN)
            {
                if (AuthorizedUser == null)
                {
                    return RedirectToAction("Login");
                }
                return RedirectToAction("Index", "Home");
            }


            return View(new AddPurchase());
        }

        [HttpPost]
        public IActionResult PurchaseMovie(AddPurchase purchase)

        {
            if (AuthorizedUser == null || AuthorizedUser.PersonType == PersonType.ADMIN)
            {
                if (AuthorizedUser == null)
                {
                    return RedirectToAction("Login");
                }
                return RedirectToAction("Index", "Home");
            }

            int userId = AuthorizedUser.Id;


            purchase.UserId = userId;


            if (ModelState.IsValid)
            {

                purchase.SelectedMovieIds = Request.Form["SelectedMovieIds"].Select(int.Parse).ToList();
                int purchaseId = PurchaseManager.AddNewPurchase(purchase);
                if (purchaseId != -1)
                {

                    int result = MoviePurchaseManager.AddNewMoviePurchase(new Models.DTO.MoviePurchase
                    {
                        SelectedMovieIds = purchase.SelectedMovieIds,
                        PurchaseId = purchaseId

                    });
                    if (result != -1) {
                        return RedirectToAction("UserPurchases");

                    }
                    else
                    {

                        return View(purchase);

                    }




                }

                return RedirectToAction("UserPurchases");
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    if (errors.Any())
                    {
                        foreach (var error in errors)
                        {
                            var errorMessage = error.ErrorMessage;
                            var exception = error.Exception;

                        }
                    }
                }

                return View(purchase);

            }


        }
        public IActionResult UserPurchases()
        {
            if (AuthorizedUser == null || AuthorizedUser.PersonType == PersonType.ADMIN)
            {
                if (AuthorizedUser == null)
                {
                    return RedirectToAction("Login");
                }
                return RedirectToAction("Index", "Home");
            }
            int userId = AuthorizedUser.Id;
            Person user = PersonsMenager.GetUserById(userId);


            List<Purchase> purchases = PurchaseManager.GetPurchasesByUserId(userId);
            UserPurchases userpurchases = new UserPurchases()
            {
                Purchases = purchases,
                UserId = userId,
                UserName = user.Name
                

            };
            return View(userpurchases);
        }
        public IActionResult Search(string keyword)
        {
            if (keyword == null)
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetString("SearchTerm", keyword);
            var result = MovieManager.SearchMovies(keyword);
            ViewBag.Keyword = keyword;
            return View(result);
        }
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
        public IActionResult Index()
        {
            var searchTerm = HttpContext.Session.GetString("SearchTerm");
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index", "Home");
            }
            var result=  MovieManager.SearchMovies(searchTerm);
            ViewBag.Keyword = searchTerm;

            return View(result);
        }
    }
}
