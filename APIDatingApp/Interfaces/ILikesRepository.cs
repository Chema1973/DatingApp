using APIDatingApp.DTOs;
using APIDatingApp.Entities;
using APIDatingApp.Helpers;

namespace APIDatingApp.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams);
    }
}