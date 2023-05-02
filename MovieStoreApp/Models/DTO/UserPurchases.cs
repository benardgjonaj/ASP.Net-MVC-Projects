namespace MovieStoreApp.Models.DTO
{
    public class UserPurchases
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<Purchase> Purchases { get; set; }
    }
}
