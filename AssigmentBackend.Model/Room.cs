namespace AssigmentBackend.Model
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }

        public Room(int id, string name, int capacity)
        {
            this.Id = id;
            this.Name = name;
            this.Capacity = capacity;
        }
    }
}
