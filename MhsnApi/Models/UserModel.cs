using System.ComponentModel.DataAnnotations;

namespace MhsnApi.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
