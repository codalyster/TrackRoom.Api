using TrackRoom.DataAccess.Contexts;
using TrackRoom.DataAccess.IRepository;
using TrackRoom.DataAccess.IUnitOfWorks;
using TrackRoom.DataAccess.Models;
using TrackRoom.DataAccess.Repository;
using TrackRoom.DataAccess.Repsitory;

namespace TrackRoom.DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();
        private IApplicationUserRepository _applicationUserRepository;
        private Repository<Meeting> _meetingRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IApplicationUserRepository ApplicationUserRepository =>
            _applicationUserRepository ??= new ApplicationUserRepository(_dbContext);

        public Repository<Meeting> MeetingRepository =>
            _meetingRepository ??= new Repository<Meeting>(_dbContext);

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new Repository<T>(_dbContext);
                _repositories[type] = repoInstance;
            }
            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

}
