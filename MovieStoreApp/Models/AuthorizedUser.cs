using MovieStoreApp.Models.Enums;

namespace MovieStoreApp.Models
{
    public class AuthorizedUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public PersonType PersonType { get; set; }
        public static AuthorizedUser GetBySession(ISession session)
        {
            if (session.Keys.Any(x => x == "email"))
            {
                var authorizedUser = new AuthorizedUser();
                authorizedUser.Email = session.GetString("email");
                authorizedUser.Id = session.GetInt32("id").Value;
                authorizedUser.PersonType = (PersonType)(session.GetInt32("personType").Value);
                return authorizedUser;
            }
            return null;

        }
       
    }
}
