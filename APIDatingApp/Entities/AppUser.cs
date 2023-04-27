using System.ComponentModel.DataAnnotations;

namespace APIDatingApp.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        // [Required]
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}