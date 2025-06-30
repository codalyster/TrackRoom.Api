using TrackRoom.DataAccess.IRepository;
using TrackRoom.DataAccess.Models;
using TrackRoom.DataAccess.Repsitory;


namespace TrackRoom.DataAccess.IUnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        Repository<T> Repository<T>() where T : ModelBase;

        Task<int> Complete();


        IApplicationUserRepository ApplicationUserRepository { get; }
    }
}
