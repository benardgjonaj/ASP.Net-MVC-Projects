using MovieStoreApp.Models.Enums;
using System.Data.SqlClient;

namespace MovieStoreApp.Models.DAL
{
   
    public class Person:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public  string Password { get; set; }
        public PersonType PersonType { get; set;}
        public static bool Insert(Person p)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using(SqlCommand cmd=new SqlCommand("INSERT INTO Persons (CreatedOn, IsDeleted, Name, Surname, Email, Password, PersonType) VALUES " +
                        "(@CreatedOn,@IsDeleted, @Name, @Surname, @Email, @Password, @PersonType)", con))
                    {
                        cmd.Parameters.AddWithValue("@Name", p.Name);
                        cmd.Parameters.AddWithValue("@Surname", p.Surname);
                        cmd.Parameters.AddWithValue("@Email", p.Email);
                        cmd.Parameters.AddWithValue("@Password", p.Password);
                        cmd.Parameters.AddWithValue("@PersonType", PersonType.USER);
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IsDeleted", 0);
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
        public static Person Login(string email, string password)
        {
            Person result = null;
            try
            {
                using(var sqlCon=new  SqlConnection(Tools.ConnectionString))
                {
                    using(var cmd=new SqlCommand("SELECT * FROM Persons WHERE Email=@Email AND Password=@Password", sqlCon))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@password", password);
                        sqlCon.Open();
                        using(var reader=cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                result = new Person()
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString(),
                                    Surname = reader["Surname"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    PersonType = (PersonType)reader["PersonType"]
                                };
                            }
                        }
                        sqlCon.Close();
                    }
                }


            }
            catch (Exception ex)
            {

                
            }
            return result;
        }
        internal static Person GetByEmail(string email)
        {
            Person result = null;
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Persons WHERE Email=@Email", con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new Person()
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString(),
                                    Surname = reader["Surname"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    PersonType = (PersonType)reader["PersonType"]
                                };
                            }
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
        public static Person GetUserById(int id) {
            Person result = null;
            try
            {
                using (SqlConnection con = new SqlConnection(Tools.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Persons WHERE Id=@Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new Person()
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString(),
                                    Surname = reader["Surname"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    PersonType = (PersonType)reader["PersonType"]
                                };
                            }
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
       
    }
}
