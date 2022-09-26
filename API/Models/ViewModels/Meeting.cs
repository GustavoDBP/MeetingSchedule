using System;

namespace MeetingSchedule.Models.ViewModels
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Room Room { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
