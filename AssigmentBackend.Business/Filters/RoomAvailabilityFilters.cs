namespace AssigmentBackend.Business.Filters
{
    public class RoomAvailabilityFilters
    {
        public DateTime DateTime { get; set; }

        public int Duration { get; set; }

        public int? MinimumRoomCapacity { get; set; }
    }
}
