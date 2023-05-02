using System.ComponentModel.DataAnnotations;

namespace MovieStoreApp.Models.DTO
{
    public class Person
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Duehet vendosur Emri")]
        [StringLength(50, ErrorMessage = "Maksimalisht emri duhet te ket 50 karaktere")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Duhet vendosur Mbiemri")]
        [StringLength(50, ErrorMessage = "Maksimalisht emri duhet te ket 50 karaktere")]
        public string Surname { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class PersonLoginRequestModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
