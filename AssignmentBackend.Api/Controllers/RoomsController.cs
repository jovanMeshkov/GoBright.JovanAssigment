using AssigmentBackend.Business.Abstractions;
using AssigmentBackend.Business.DTOs;
using AssigmentBackend.Business.Filters;
using AssigmentBackend.Business.Services;
using AssigmentBackend.Model;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentBackend.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomGetService _roomGetService;
        private readonly IRoomAvailabilityService _roomAvailabilityService;

        public RoomsController(
            IRoomGetService roomGetService,
            IRoomAvailabilityService roomAvailabilityService
        )
        {
            _roomGetService = roomGetService;
            _roomAvailabilityService = roomAvailabilityService;
        }

        [HttpGet("{id}")]
        public Task<Room> Get(int id)
        {
            return _roomGetService.GetAsync(id);
        }

        [HttpGet]
        public Task<List<Room>> GetAll()
        {
            return _roomGetService.GetAllAsync();
        }

        [HttpPost("Availability")]
        public Task<List<RoomAvailability>> GetAvailability([FromBody] RoomAvailabilityFilters filters)
        {
            return _roomAvailabilityService.GetAsync(filters);
        }
    }
}
