using Microsoft.AspNetCore.Mvc;
using MovieStoreApp.Models;
using MovieStoreApp.Models.BLL;
using MovieStoreApp.Models.DTO;

namespace MovieStoreApp.Controllers
{
    public class WishlistItemsController : BaseController
    {
        
        [HttpPost]
        public IActionResult AddToWishlist(int id)
        {
            int userId = AuthorizedUser.Id;
            WishlistItem wishlistItem = new WishlistItem
            {
                MovieId= id,
                UserId= userId
                
            };
            try
            {
                if (ModelState.IsValid) {
                    WishlistItemManger.Add(wishlistItem);
                    TempData["SuccessMessage"] = "Movie added to wishlist successfully!";
                }
                return RedirectToAction("Index", "Movie");

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("",$"{ ex.Message}");
                TempData["Unsuccessfully"]= ex.Message;
                return RedirectToAction("Index", "Movie");
            }
            
        }
        public IActionResult Index()
        {
            return View(WishlistItemManger.GetWishlistItems());
        }
    }
}
