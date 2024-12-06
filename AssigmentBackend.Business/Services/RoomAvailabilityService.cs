using AssigmentBackend.Business.Abstractions;
using AssigmentBackend.Business.Configs;
using AssigmentBackend.Business.DTOs;
using AssigmentBackend.Business.Filters;
using AssigmentBackend.Business.Utilities;
using AssigmentBackend.Database;
using AssigmentBackend.Model;
using Microsoft.Extensions.Options;

namespace AssigmentBackend.Business.Services
{
    public class RoomAvailabilityService : IRoomAvailabilityService
    {
        private readonly DbContext _dbContext;
        private readonly IOptionsMonitor<RoomConfig> _roomConfig;

        public RoomAvailabilityService(
            DbContext dbContext,
            IOptionsMonitor<RoomConfig> roomConfig)
        {
            _dbContext = dbContext;
            _roomConfig = roomConfig;
        }

        public Task<List<RoomAvailability>> GetAsync(RoomAvailabilityFilters filters)
        {
            var slots = _roomConfig.CurrentValue.AvailabilitySlots;
            var interval = _roomConfig.CurrentValue.AvailabilityInterval;

            // Used for relevant bookings retrieval
            //
            var searchInterval = new DateTimeInterval
            {
                Start = filters.DateTime,
                End = filters.DateTime.AddMinutes(
                    slots * filters.Duration - ((slots - 1) * interval))
            };

            // Flatten bookings data to be query friendly
            //
            var bookingsFlatten = _dbContext.Bookings
                .SelectMany(x => x.RoomIds, (b, roomId) => new
                {
                    b.Id,
                    b.Subject,
                    b.Start,
                    b.End,
                    RoomId = roomId
                });

            // Extract relevant room and bookings data
            //
            var roomsQuery = from room in _dbContext.Rooms
                             where filters.MinimumRoomCapacity != null &&
                                   room.Capacity >= filters.MinimumRoomCapacity
                             select room;

            var bookingsQuery = from room in _dbContext.Rooms
                                join booking in bookingsFlatten
                                    on room.Id equals booking.RoomId
                                where (filters.MinimumRoomCapacity != null &&
                                       room.Capacity >= filters.MinimumRoomCapacity)
                                      &&
                                      searchInterval.Start <= booking.Start &&
                                      booking.Start <= searchInterval.End
                                select booking;

            var rooms = roomsQuery.ToList();
            var bookings = bookingsQuery.ToList();

            // Find rooms availability
            //
            var roomsAvailability = new List<RoomAvailability>(rooms.Count);

            foreach (var room in rooms)
            {
                var roomBookings = bookings
                    .Where(x => x.RoomId == room.Id)
                    .OrderBy(x => x.Start)
                    .ToList();

                var roomAvailability = new RoomAvailability
                {
                    Id = room.Id,
                    Name = room.Name,
                    Slots = new List<RoomAvailabilitySlot>(slots)
                };

                // Slots building
                //
                var currentSlotStartDate = searchInterval.Start;
                for (var i = 0; i < slots; i++)
                {
                    var currentSlotEndDate = currentSlotStartDate.AddMinutes(filters.Duration);

                    var currentSlotInterval = new DateTimeInterval(currentSlotStartDate, currentSlotEndDate);

                    // Determine availability for current slot
                    //
                    var slotOccupied = roomBookings?.Any(rb => currentSlotInterval.Overlap((rb.Start, rb.End)))
                        ?? false;

                    var slot = new RoomAvailabilitySlot
                    {
                        Start = currentSlotStartDate,
                        End = currentSlotEndDate,
                        Available = slotOccupied == false
                    };

                    roomAvailability.Slots.Add(slot);

                    currentSlotStartDate = currentSlotStartDate.AddMinutes(interval);
                }

                roomsAvailability.Add(roomAvailability);
            }

            return Task.FromResult(roomsAvailability);
        }


    }
}
