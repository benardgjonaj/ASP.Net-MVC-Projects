using MovieStoreApp.Models.DAL;

namespace MovieStoreApp.Models.BLL
{
    public class MoviePurchaseManager
    {
        public static int AddNewMoviePurchase(DTO.MoviePurchase moviepurchase)
        {
            DAL.MoviePurchase moviePurchaseToInsert = new DAL.MoviePurchase()
            {
                PurchaseId = moviepurchase.PurchaseId,
                SelectedMovieIds = moviepurchase.SelectedMovieIds,
            };
            int result = MoviePurchase.Insert(moviePurchaseToInsert);
            if(result!=-1)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
       
    }
}
