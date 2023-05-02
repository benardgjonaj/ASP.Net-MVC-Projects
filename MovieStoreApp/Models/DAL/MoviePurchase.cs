using System.Data.SqlClient;

namespace MovieStoreApp.Models.DAL
{
    public class MoviePurchase
    {
        public int MovieId { get; set; }
        public int PurchaseId { get; set; }
        public Movie Movie { get; set; }
        public Purchase Purchase { get; set; }
        public List<int> SelectedMovieIds { get; set; }

        public static int Insert(MoviePurchase moviePurchase)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    con.Open();

                    foreach (int movieId in moviePurchase.SelectedMovieIds)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO MoviePurchases (MovieId,PurchaseId) " +
                                                                "VALUES (@MovieId, @PurchaseId)", con))
                        {

                            cmd.Parameters.AddWithValue("MovieId", movieId);
                            cmd.Parameters.AddWithValue("PurchaseId", moviePurchase.PurchaseId);

                            int added = cmd.ExecuteNonQuery() == 1 ? 1 : -1;
                            if (added!=1)
                            {
                                return added;
                            }
                        }
                    }

                    return 1;
                }
            }
            catch (Exception ex)
            {
                
            }
            return -1;
        }
        public static List<int> GetSelectedMovieIds(int movieId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT MovieId FROM MoviePurchase WHERE MovieId = @MovieId and IsDeleted=0 ", con))
                    {
                        cmd.Parameters.AddWithValue("@MovieId", movieId);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        List<int> selectedMovieIds = new List<int>();
                        while (reader.Read())
                        {
                            selectedMovieIds.Add(reader.GetInt32(0));
                        }
                        con.Close();
                        return selectedMovieIds;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new List<int>();
        }
        public static bool Update(MoviePurchase moviepurchase)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE MoviePurchase SET MovieId = @MovieId WHERE PurchaseId = @PurchaseId", con))
                    {
                        cmd.Parameters.AddWithValue("PurchaseId", moviepurchase.PurchaseId);
                        cmd.Parameters.AddWithValue("MovieId", moviepurchase.MovieId);
                        con.Open();
                        bool updated = cmd.ExecuteNonQuery() == 1;
                        con.Close();
                        return updated;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle the exception
            }
            return false;
        }
        
       
    }
}

