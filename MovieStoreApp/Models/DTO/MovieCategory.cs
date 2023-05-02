namespace MovieStoreApp.Models.DTO
{
    public class MovieCategory
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
