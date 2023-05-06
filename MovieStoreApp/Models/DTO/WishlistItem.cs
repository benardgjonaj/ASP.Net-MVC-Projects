namespace MovieStoreApp.Models.DTO
{
    public class WishlistItem
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
