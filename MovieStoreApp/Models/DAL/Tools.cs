namespace MovieStoreApp.Models.DAL
{
    public static class Tools
    {
        public static string ConnectionString {get; private set;}
        public static void SetConnectionString(string  connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
