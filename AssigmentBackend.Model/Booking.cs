namespace AssigmentBackend.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<int> RoomIds { get; set; }

        public Booking(int id, string subject, DateTime start, DateTime end, List<int> roomIds)
        {
            this.Id = id;
            this.Subject = subject;
            this.Start = start;
            this.End = end;
            this.RoomIds = roomIds;
        }
    }
}
