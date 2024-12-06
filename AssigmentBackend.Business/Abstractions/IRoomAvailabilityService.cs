using AssigmentBackend.Business.DTOs;
using AssigmentBackend.Business.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssigmentBackend.Business.Abstractions
{
    public interface IRoomAvailabilityService
    {
        Task<List<RoomAvailability>> GetAsync(RoomAvailabilityFilters filters);
    }
}
