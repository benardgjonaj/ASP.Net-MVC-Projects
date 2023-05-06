using MovieStoreApp.Models.DTO;
using System.Data.SqlClient;

namespace MovieStoreApp.Models.DAL
{
    public class WishlistItem : BaseEntity
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Quantity { get; set; }

        public static int Insert(WishlistItem wishlistItem)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO WishlistItems (UserId, MovieId, Quantity) " +
                                                            "VALUES (@UserId, @MovieId, @Quantity); select @@Identity;", con))
                    {
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IsDeleted", 0);
                        cmd.Parameters.AddWithValue("@UserId", wishlistItem.UserId);
                        cmd.Parameters.AddWithValue("@MovieId", wishlistItem.MovieId);
                        cmd.Parameters.AddWithValue("@Quantity", wishlistItem.Quantity);
                        con.Open();
                        var res = cmd.ExecuteScalar();
                        int.TryParse(res.ToString(), out result);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        public static List<WishlistItem> GetWishlistItems()
        {
            List<WishlistItem> result= new List<WishlistItem>();
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from WishlistItems Where IsDeleted=0", con))
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var wishlistItem = new WishlistItem
                            {
                                Id = (int)reader["Id"],
                                UserId = (int)reader["UserId"],
                                MovieId = (int)reader["MovieId"],
                                

                            };
                            
                            result.Add(wishlistItem);
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
        public static WishlistItem GetWishlistItemsByMoveId(int movieId)
        {
            WishlistItem wishlistItem = null;
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from WishlistItems where MovieId = @movieId AND isDeleted = 0 ", con))
                    {
                        cmd.Parameters.AddWithValue("@MovieId", movieId);
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            wishlistItem = new WishlistItem
                            {
                                Id = (int)reader["Id"],
                                MovieId = (int)reader["MovieId"],
                                UserId = (int)reader["UserId"],
                              
                            };
                           
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return wishlistItem;
        }
    }
}
