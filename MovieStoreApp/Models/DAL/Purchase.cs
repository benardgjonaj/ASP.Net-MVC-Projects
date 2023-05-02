using System.Data.SqlClient;

namespace MovieStoreApp.Models.DAL
{
    public class Purchase:BaseEntity
    {
     
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Description { get; set; }
        public List<Movie> Movies { get; set; }
        public static int InsertPurchase(Purchase purchase)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Purchases (UserId, Description) " +
                                                            "VALUES (@UserId, @Description); " +
                                                            "SELECT SCOPE_IDENTITY();", con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", purchase.UserId);
                        cmd.Parameters.AddWithValue("@Description", purchase.Description);
                        con.Open();
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return -1;
        }
        public static Purchase GetPurchaseById(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Purchases WHERE Id = @Id;", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Purchase
                                {
                                    Id = (int)reader["Id"],
                                    UserId = (int)reader["UserId"],
                                    MovieId = (int)reader["MovieId"],
                                    Description =(string) reader["Description"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return null;
        }
        public static List<Purchase> GetPurchases()
        {
            List<Purchase> purchases = new List<Purchase>();

            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Purchases Where IsDeleted=0;", con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Purchase purchase = new Purchase
                                {
                                    Id = (int)reader["Id"],
                                    UserId = (int)reader["UserId"],
                                    MovieId = (int)reader["MovieId"],
                                    Description =(string) reader["Description"] 
                                };
                                purchases.Add(purchase);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return purchases;
        }
        public static bool DeletePurchase(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Purchases SET IsDeleted=1 WHERE Id = @Id", con))
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
                
            }

            return false;
        }
      
        public static List<Purchase> GetPurchasesByUserId(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Purchases.* " +
                                                             "FROM Purchases " +
                                                             "INNER JOIN Persons ON Persons.Id = Purchases.UserId " +
                                                             "WHERE Persons.Id = @UserId", con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Purchase> purchases = new List<Purchase>();
                            while (reader.Read())
                            {
                                Purchase purchase = new Purchase
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),

                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                };
                                purchases.Add(purchase);
                            }
                            return purchases;
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
    }
}
