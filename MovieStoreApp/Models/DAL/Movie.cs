using MovieStoreApp.Models.DAL;
using MovieStoreApp.Models.DTO;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Net.WebSockets;
namespace MovieStoreApp.Models.DAL
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public List<Category> Categories { get; set; }

        public static int Insert(Movie movie)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Movies (CreatedOn, IsDeleted, Title, Director, Description, ReleaseYear, Price) " +
                                                            "VALUES (@CreatedOn, @IsDeleted, @Title, @Director, @Description, @ReleaseYear, @Price); select @@Identity;", con))
                    {
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IsDeleted", 0);
                        cmd.Parameters.AddWithValue("@Title", movie.Title);
                        cmd.Parameters.AddWithValue("@Director", movie.Director);
                        cmd.Parameters.AddWithValue("@Description", movie.Description);
                        cmd.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
                        cmd.Parameters.AddWithValue("@Price", movie.Price);
                        con.Open();
                        var res = cmd.ExecuteScalar();
                        int.TryParse(res.ToString(), out result);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return result;
        }
        public static Movie GetMovieById(int id)
        {
            Movie movie = null;
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Movies WHERE Id = @Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            movie = new Movie
                            {
                                Id = (int)reader["Id"],
                                Title = (string)reader["Title"],
                                Director = (string)reader["Director"],
                                Description = (string)reader["Description"],
                                ReleaseYear = (int)reader["ReleaseYear"],
                                Price = (double)reader["Price"]
                            };
                            if (!string.IsNullOrEmpty(reader["Image"].ToString()))
                            {
                                movie.Image = (string)reader["Image"];
                            }
                        }

                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return movie; ;
        }
        public static bool Update(Movie movie = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Movies SET Title=@Title,Director=@Director, Description=@Description, ReleaseYear=@ReleaseYear, Price=@Price, Image=@Image WHERE Id=@Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Title", movie.Title);
                        cmd.Parameters.AddWithValue("@Director", movie.Director);
                        cmd.Parameters.AddWithValue("@Description", movie.Description);
                        cmd.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
                        cmd.Parameters.AddWithValue("@Price", movie.Price);
                        if (movie.Image != null)
                        {
                            cmd.Parameters.AddWithValue("@Image", movie.Image);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Image", DBNull.Value);
                        }
                        cmd.Parameters.AddWithValue("@Id", movie.Id);
                        con.Open();
                        bool updated = cmd.ExecuteNonQuery() == 1;
                        con.Close();
                        return updated;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
            }
            return false;
        }
        public static bool DeleteMovie(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Movies SET IsDeleted=1 WHERE Id = @Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        bool deleted = cmd.ExecuteNonQuery() == 1;
                        con.Close();
                        return deleted;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
            }
            return false;
        }
        public static List<Movie> GetMovies()
        {
            List<Movie> result = new List<Movie>();
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from Movies Where IsDeleted=0", con))
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var movie = new Movie
                            {
                                Id = (int)reader["Id"],
                                Title = (string)reader["Title"],
                                Director = (string)reader["Director"],
                                Description = (string)reader["Description"],
                                ReleaseYear = (int)reader["ReleaseYear"],
                                Price = (double)reader["Price"]

                            };
                            if (!string.IsNullOrEmpty(reader["Image"].ToString()))
                            {
                                movie.Image = (string)reader["Image"];
                            }
                            result.Add(movie);
                        }
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {


            }
            return result;
        }
        public static List<Movie> GetMoviesBypurchaseId(int id)
        {
            List<Movie> movies = new List<Movie>();

            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Movies.* " +
                                                             "FROM Movies " +
                                                             "INNER JOIN MoviePurchases ON Movies.Id = MoviePurchases.MovieId " +
                                                             "WHERE MoviePurchases.PurchaseId = @PurchaseId", con))
                    {
                        cmd.Parameters.AddWithValue("@PurchaseId", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Movie movie = new Movie
                                {
                                    Id = (int)reader["Id"],
                                    Title = (string)reader["Title"],
                                    Description = (string)reader["Description"],
                                    Director = (string)reader["Director"],
                                    ReleaseYear = (int)reader["ReleaseYear"],
                                    Price = (double)reader["Price"]
                                };
                                movies.Add(movie);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
            }

            return movies;

        }
        internal static void SetImagePath(int id, string newFileName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("update Movies set image = @image where id = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@image", newFileName);
                        cmd.Parameters.AddWithValue("@id", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static List<Movie> GetTop6FeaturedMovies()
        {
            List<Movie> result = new List<Movie>();
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select Top 6* from Movies where isfeatured = 1 order by id desc", con))
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var movie = new Movie
                            {
                                Id = (int)reader["Id"],
                                Title = (string)reader["Title"],
                                Director = (string)reader["Director"],
                                Description = (string)reader["Description"],
                                ReleaseYear = (int)reader["ReleaseYear"],
                                Price = (double)reader["Price"]

                            };
                            if (!string.IsNullOrEmpty(reader["Image"].ToString()))
                            {
                                movie.Image = (string)reader["Image"];
                            }
                            result.Add(movie);
                        }
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {


            }
            return result;
        }
        public static Movie GetByTitle(string title)
        {
            Movie movie = null;
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from Movies where Title = @title AND isDeleted = 0 ", con))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            movie = new Movie
                            {
                                Id = (int)reader["Id"],
                                Title = (string)reader["Title"],
                                Director = (string)reader["Director"],
                                Description = (string)reader["Description"],
                                Price = (double)reader["Price"]
                            };
                            if (!string.IsNullOrEmpty(reader["Image"].ToString()))
                            {
                                movie.Image = (string)reader["Image"];
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return movie;

        }
        public static List<Movie> SearchMovies(string keywords)
        {
            List<Movie> movies = new List<Movie>();

            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string sql = "SELECT Id, Title, Director, Description, ReleaseYear, Price, Image FROM Movies WHERE ";
                        if (keywords.Length >= 4)
                        {
                            sql += "(Title LIKE @Keywords OR ReleaseYear = @Year)";
                            cmd.Parameters.AddWithValue("@Keywords", "%" + keywords + "%");
                            if (Int32.TryParse(keywords, out int year))
                            {
                                cmd.Parameters.AddWithValue("@Year", year);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Year", -1); // dummy value that won't match any year
                            }
                        }
                        else
                        {
                            return movies; // return an empty list if the search term is too short
                        }
                        cmd.CommandText = sql;
                        cmd.Connection = con;
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Movie movie = new Movie()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Title = reader["Title"].ToString(),
                                    Director = reader["Director"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    ReleaseYear = Convert.ToInt32(reader["ReleaseYear"]),
                                    Price = Convert.ToDouble(reader["Price"])
                                };
                                if (!string.IsNullOrEmpty(reader["Image"].ToString()))
                                {
                                    movie.Image = (string)reader["Image"];
                                }

                                movies.Add(movie);
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return movies;
        }
        public static List<Movie> GetMoviesByCategoryId(int categoryId)
        {
            List<Movie> movies = new List<Movie>();

            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string sql = "SELECT m.Id, m.Title, m.Director, m.Description, m.ReleaseYear, m.Price, m.Image "  +
                                     "FROM Movies m " +
                                     "INNER JOIN MovieCategories mc ON m.Id = mc.MovieId " +
                                     "WHERE mc.CategoryId = @CategoryId";
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmd.CommandText = sql;
                        cmd.Connection = con;
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Movie movie = new Movie()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Title = reader["Title"].ToString(),
                                    Director = reader["Director"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    ReleaseYear = Convert.ToInt32(reader["ReleaseYear"]),
                                    Price = Convert.ToDouble(reader["Price"])
                                }; 
                                if (!string.IsNullOrEmpty(reader["Image"].ToString()))
                                {
                                    movie.Image = (string)reader["Image"];
                                }

                                movies.Add(movie);
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return movies;
        }
    }
}
