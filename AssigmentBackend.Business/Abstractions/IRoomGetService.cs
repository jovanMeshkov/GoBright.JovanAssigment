using AssigmentBackend.Database;
using AssigmentBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssigmentBackend.Business.Abstractions
{
    public interface IRoomGetService
    {
        Task<Room> GetAsync(int id);
        Task<List<Room>> GetAllAsync();
    }
}
