using MovieStoreApp.Models.DTO;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Security.Policy;

namespace MovieStoreApp.Models.DAL
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MovieCount { get; set; }
        public  List<Movie> Movies { get; set; }
     
        public static bool Insert(Category category)
        {
            try
            {
                using(SqlConnection con=new SqlConnection(Tools.ConnectionString))
                {
                    using(SqlCommand cmd=new SqlCommand("INSERT INTO Categories (CreatedOn, IsDeleted,Name,Description) " +
                        "VALUES (@CreatedOn,@IsDeleted, @Name, @Description)", con))
                    {
                        cmd.Parameters.AddWithValue("CreatedOn", DateTime.Now);
                        cmd.Parameters.AddWithValue("IsDeleted", 0);
                        cmd.Parameters.AddWithValue("Name", category.Name);
                        cmd.Parameters.AddWithValue("Description", category.Description);
                        con.Open();
                        bool added = cmd.ExecuteNonQuery() == 1;
                        con.Close();
                        return added;
                    }
                }

            }
            catch (Exception ex)
            {

               
            }
            return false;
        }
        public static Category GetCategoryById(int id) {
            Category result = null;
            try
            {
                using(SqlConnection con=new SqlConnection(Tools.ConnectionString)) {
                    using(SqlCommand cmd=new SqlCommand("SELECT * FROM Categories WHERE Id = @Id and IsDeleted = 0", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open() ;
                        SqlDataReader reader= cmd.ExecuteReader();
                        if(reader.Read())
                        {
                            result = new Category()
                            {
                                Id = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Description = (string)reader["Description"]
                            };
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
        public static bool Update(Category category)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Categories SET Name=@Name, Description=@Description WHERE Id=@Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", category.Id);
                        cmd.Parameters.AddWithValue("@Name", category.Name);
                        cmd.Parameters.AddWithValue("@Description", category.Description);
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
        public static bool DeleteCategory(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Categories SET IsDeleted=1 WHERE Id = @Id", con))
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
        public static List<Category> GetCategories()
        {
            List<Category> result = new List<Category>();
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using(SqlCommand cmd=new SqlCommand("select * from Categories where IsDeleted = 0", con))
                    {
                        con.Open();
                        SqlDataReader reader=cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            result.Add(new Category
                            {
                                Id = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Description = (string)reader["Description"]
                            });
                        }
                        con.Close() ;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
        public List<Category> GetCategoriesWithMovies()
        {
            var categories = new List<Category>();
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(@" SELECT c.CategoryId, c.CategoryName, COUNT(m.MovieId) AS MovieCount
            FROM Category c
            LEFT JOIN MovieCategory mc ON mc.CategoryId = c.Id
            LEFT JOIN Movie m ON m.Id = mc.MovieId
            GROUP BY c.Id, c.Name
            ORDER BY c.Name", con))
                    {
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var category = new Category
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                MovieCount = reader.GetInt32(2)
                            };

                            categories.Add(category);
                        }

                        con.Close();
                    }
                }
            }

                    
            catch (Exception ex)
            {

                throw;
            }
           
            

            return categories;
        }
        public static Category GetByName(string name)
        {
            Category category = null;
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from Categories where Name = @Name AND isDeleted = 0 ", con))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            category = new Category
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                               
                                Description = (string)reader["Description"],
                               
                            };
                           
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return category;

        }
        public static List<Category> GetCategoriesByMovieId(int movieId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Categories.* FROM Categories " +
                        "INNER JOIN MovieCategories ON Categories.Id = MovieCategories.CategoryId " +
                        "WHERE MovieCategories.MovieId = @MovieId", con))
                    {
                        cmd.Parameters.AddWithValue("@MovieId", movieId);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Category> categories = new List<Category>();
                            while (reader.Read())
                            {
                                Category category = new Category
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                };
                                categories.Add(category);
                            }
                            return categories;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
            }
            return null;
        }
        public static List<int> SearchCategories(string keywords)
        {
            List<int> categoryIds = new List<int>();

            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string sql = "SELECT Id FROM Categories WHERE Name LIKE @Keywords";
                        cmd.Parameters.AddWithValue("@Keywords", "%" + keywords + "%");
                        cmd.CommandText = sql;
                        cmd.Connection = con;
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int categoryId = Convert.ToInt32(reader["Id"]);
                                categoryIds.Add(categoryId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
            }

            return categoryIds;
        }



    }
}
