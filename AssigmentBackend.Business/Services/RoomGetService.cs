using AssigmentBackend.Business.Abstractions;
using AssigmentBackend.Database;
using AssigmentBackend.Model;

namespace AssigmentBackend.Business.Services
{
    public class RoomGetService : IRoomGetService
    {
        private readonly DbContext _dbContext;

        public RoomGetService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Room> GetAsync(int id)
        {
            return _dbContext.Rooms.First(x => x.Id == id);
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return _dbContext.Rooms.ToList();
        }
    }
}
