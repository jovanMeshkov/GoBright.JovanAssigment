using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssigmentBackend.Business.DTOs
{
    public class RoomAvailability
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RoomAvailabilitySlot> Slots { get; set; }
    }

    public class RoomAvailabilitySlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Available { get; set; }
    }
}
