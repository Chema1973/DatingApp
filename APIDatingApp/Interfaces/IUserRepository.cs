using APIDatingApp.DTOs;
using APIDatingApp.Entities;
using APIDatingApp.Helpers;

namespace APIDatingApp.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        // Task<bool> SaveAllAsync();
        // --> Se quita porque lo hará "IUnitOfWork"
        
        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetUserByUserNameAsync(string username);

        // Task<IEnumerable<MemberDTO>> GetMembersAsync();
        Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);

        Task<MemberDTO> GetMemberAsync(string username);

        Task<string> GetUserGender(string username);
    }
}