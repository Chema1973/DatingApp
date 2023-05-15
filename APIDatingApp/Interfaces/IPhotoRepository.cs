using APIDatingApp.DTOs;
using APIDatingApp.Entities;

namespace APIDatingApp.Interfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<PhotoForApprovalDTO>> GetUnapprovedPhotos();
        Task<Photo> GetPhotoById(int id);
        void RemovePhoto(Photo photo);
        }
}