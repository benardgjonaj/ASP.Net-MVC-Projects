using MovieStoreApp.Models.DAL;
using MovieStoreApp.Models.Enums;
namespace MovieStoreApp.Models.BLL
{
    public class PersonsMenager
    {
        public static bool Register(DTO.Person person)
        {
            var existingPerson = DAL.Person.GetByEmail(person.Email);
            if (existingPerson != null)
            {
                return false;
            }
            return DAL.Person.Insert(new DAL.Person()
            {
                Name = person.Name,
                Surname = person.Surname,
                Email = person.Email,
                Password = person.Password,
                PersonType = PersonType.USER
            });
        }

        public static AuthorizedUser Login(string email, string password)
        {
            var person = DAL.Person.Login(email, password);
            if (person != null)
            {
                return new AuthorizedUser
                {
                    Email = person.Email,
                    Id = person.Id,
                    PersonType = person.PersonType
                };
            }
            return null;
        }
        public static DTO.Person GetUserById(int id)
        {
            Person user = Person.GetUserById(id);
            if (user != null)
            {
                return new DTO.Person()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Password = user.Password,
                    Id = user.Id
                };
            }
            return null;
        }
    }
}
