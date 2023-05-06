using MovieStoreApp.Models.DTO;

namespace MovieStoreApp.Models.BLL
{
    public class WishlistItemManger
    {
        public static DTO.WishlistItem Add(DTO.WishlistItem model) {
            DAL.WishlistItem wishlistItemToInsert = new DAL.WishlistItem
            {
                UserId = model.UserId,
                MovieId = model.MovieId,
                Quantity = 1

            };
            var existiwishlistItem = DAL.WishlistItem.GetWishlistItemsByMoveId(wishlistItemToInsert.MovieId);
            if (existiwishlistItem==null)
            {
                int result = DAL.WishlistItem.Insert(wishlistItemToInsert);

                if (result > 0)
                {
                    return model;

                }
                return null;
            }
            else
            {
                throw new Exception("Movie is already on wishlist");
              
            }
        }
        public static List< DTO.WishlistItem> GetWishlistItems()
        {
            List<DAL.WishlistItem> dalwishlistItems = DAL.WishlistItem.GetWishlistItems();
            List<DTO.WishlistItem> dtowishlistItems = new List<DTO.WishlistItem>();
            foreach (var wishlistItem in dalwishlistItems)
            {
                dtowishlistItems.Add(new DTO.WishlistItem
                {
                    UserId = wishlistItem.UserId,
                    MovieId = wishlistItem.MovieId,
                    Movie = MovieManager.GetMovieById(wishlistItem.MovieId)

                });

            }
            if(dtowishlistItems!= null)
            {
                return dtowishlistItems;
            }
            else
            {
                return null;
            }
        }
    }
}
