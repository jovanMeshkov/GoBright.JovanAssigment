using AssigmentBackend.Model;

namespace AssigmentBackend.Database
{
    public class DbContext
    {
        public List<Room> Rooms { get; private set; }
        public List<Booking> Bookings { get; private set; }

        public DbContext()
        {
            this.Rooms = new List<Room>()
            {
                new Room(1, "Meeting Room London", 10),
                new Room(2, "Meeting Room Amsterdam", 4),
                new Room(3, "Meeting Room Paris", 20)
            };

            this.Bookings = new List<Booking>()
            {
                new Booking(1, "Meeting A", new DateTime(2024, 01, 01, 08, 00, 00), new DateTime(2024, 01, 01, 10, 00, 00), new List<int>() { 1, 3}),
                new Booking(2, "Meeting B", new DateTime(2024, 01, 01, 10, 15, 00), new DateTime(2024, 01, 01, 10, 30, 00), new List<int>() { 1, 2 }),
                new Booking(3, "Meeting C", new DateTime(2024, 01, 01, 11, 00, 00), new DateTime(2024, 01, 01, 14, 00, 00), new List<int>() { 2 })
            };
        }
    }
}
