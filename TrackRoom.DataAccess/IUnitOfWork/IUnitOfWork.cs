using TrackRoom.DataAccess.IRepository;
using TrackRoom.DataAccess.Models;
using TrackRoom.DataAccess.Repsitory;

namespace TrackRoom.DataAccess.IUnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        // Use the interface, not the implementation
        IRepository<T> Repository<T>() where T : class;

        // Commit changes to the database
        Task<int> Complete();

        // Expose specific repositories
        IApplicationUserRepository ApplicationUserRepository { get; }

        Repository<Meeting> MeetingRepository { get; }

    }
}
