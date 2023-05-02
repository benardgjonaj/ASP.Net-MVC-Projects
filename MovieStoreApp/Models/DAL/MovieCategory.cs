using MovieStoreApp.Models.DTO;
using System.Data.SqlClient;

namespace MovieStoreApp.Models.DAL
{
    public class MovieCategory:BaseEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int CategoryId { get; set; }
        public List<int> CategoryIds { get; set; }
        public Category Category { get; set; }

        public static bool Insert(MovieCategory moviecategory)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    con.Open();

                   
                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO MovieCategories (MovieId, CategoryId) VALUES (@MovieId, @CategoryId)", con))
                    {
                        foreach (int categoryId in moviecategory.CategoryIds)
                        {
                            insertCmd.Parameters.Clear();
                            insertCmd.Parameters.AddWithValue("@MovieId", moviecategory.MovieId);
                            insertCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
               
            }

            return false;
        }
        public static bool Update(MovieCategory moviecategory)
        {
            var deleted=Delete(moviecategory.MovieId);
            var added=Insert(moviecategory);
            if (deleted && added)
            {
                return true;
            }
            return false;
        }
        public static List<int> GetSelectedCategoryIds(int movieId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CategoryId FROM MovieCategories WHERE MovieId = @MovieId and IsDeleted=0 ", con))
                    {
                        cmd.Parameters.AddWithValue("@MovieId", movieId);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        List<int> selectedCategories = new List<int>();
                        while (reader.Read())
                        {
                            selectedCategories.Add(reader.GetInt32(0));
                        }
                        con.Close();
                        return selectedCategories;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return new List<int>();
        }
        public static bool Delete(int movieId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    con.Open();

                    
                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM MovieCategories WHERE MovieId = @MovieId", con))
                    {
                        deleteCmd.Parameters.AddWithValue("@MovieId", movieId);
                        deleteCmd.ExecuteNonQuery();
                    }
                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }
        public static bool DeleteCategory(int catogoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    con.Open();


                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM MovieCategories WHERE CategoryId = @CategoryId", con))
                    {
                        deleteCmd.Parameters.AddWithValue("@CategoryId", catogoryId);
                        deleteCmd.ExecuteNonQuery();
                    }
                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public static List<Movie> GetMoviesByCategoryId(int categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Movies.* FROM Movies " +
                        "INNER JOIN MovieCategories ON Movies.Id = MovieCategories.MovieId " +
                        "WHERE MovieCategories.CategoryId = @CategoryId", con))
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Movie> movies = new List<Movie>();
                            while (reader.Read())
                            {
                                Movie movie = new Movie
                                {

                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Director = reader.GetString(reader.GetOrdinal("Director")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDouble(reader.GetOrdinal("Price")),
                                    ReleaseYear=reader.GetInt32(reader.GetOrdinal("ReleaseYear"))
                                   
                                   
                                };
                                movies.Add(movie);
                            }
                            return movies;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
            return null;
        }
       
    }
}

