using APIDatingApp.Entities;

namespace APIDatingApp.Interfaces
{
    public interface ITokenService
    {
         Task<string>  CreateToken(AppUser user);
    }
}