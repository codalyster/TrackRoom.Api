using TrackRoom.DataAccess.IRepository;

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

        IMeetingRepository MeetingRepository { get; }
    }
}
