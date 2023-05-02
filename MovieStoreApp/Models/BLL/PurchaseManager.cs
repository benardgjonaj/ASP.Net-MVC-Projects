using MovieStoreApp.Models.DTO;

namespace MovieStoreApp.Models.BLL
{
    public class PurchaseManager
    {
        public static int AddNewPurchase(DTO.AddPurchase model)
        {
         DAL.Purchase purchase = new DAL.Purchase()
            {
               
                UserId = model.UserId,
                Description = model.Description,
            };
            int result=DAL.Purchase.InsertPurchase(purchase);
            if(result!=-1)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
       
        public static List<DTO.Purchase> GetPurchasesByUserId(int userId)
        {
            List<DAL.Purchase> purchasesDAL = DAL.Purchase.GetPurchasesByUserId(userId);
            List<DTO.Purchase> purchasesDTO = new List<DTO.Purchase>();
            foreach (var purchaseDAL in purchasesDAL)
            {

                DTO.Purchase purchaseDTO = new DTO.Purchase
                {
                    Id = purchaseDAL.Id,
                    UserId = purchaseDAL.UserId,
                    Description = purchaseDAL.Description,
                    CreatedDate=purchaseDAL.CreatedOn,
                    Movies = MovieManager.GetMoviesBypurchaseId(purchaseDAL.Id)
                };

                purchasesDTO.Add(purchaseDTO);
            }
            return purchasesDTO;

        }
    }
}
