namespace MovieStoreApp.Models.DTO
{
    public class MoviePurchase
    {
        
        public int PurchaseId { get; set; }
        public List<int> SelectedMovieIds { get; set; }
    }
}
