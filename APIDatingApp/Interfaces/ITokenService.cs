using APIDatingApp.Entities;

namespace APIDatingApp.Interfaces
{
    public interface ITokenService
    {
         string  CreateToken(AppUser user);
    }
}