using TrackRoom.DataAccess.Models;

namespace TrackRoom.DataAccess.IRepository
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByUserNameAsync(string username);
    }
}
