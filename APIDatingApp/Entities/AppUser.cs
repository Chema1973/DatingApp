using System.ComponentModel.DataAnnotations;
using APIDatingApp.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace APIDatingApp.Entities
{
    public class AppUser : IdentityUser<int>
    {
        // public int Id { get; set; }
        // public string UserName { get; set; }
        // public byte[] PasswordHash { get; set; }
        // --> Estas 3 propiedades están dentro de "IdentityUser"
        //     Le pasamos <int> para indicar que nuestro Id seguirá siendo un número

        // public byte[] PasswordSalt { get; set; }
        // --> Esta directamente ya no es necesaria

        public DateOnly DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<Photo> Photos { get; set; } = new(); // new List<Photo>();
        // --> Relación con la tabla "Photos" (many)

        public List<UserLike> LikedByUsers { get; set; }

        public List<UserLike> LikedUsers { get; set; }


        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }


        public ICollection<AppUserRole> UserRoles { get; set; }

        
        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }
        // --> De momento comentamos esto
    }
}