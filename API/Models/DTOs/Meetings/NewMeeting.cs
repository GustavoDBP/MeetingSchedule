using System;

namespace MeetingSchedule.Models.DTOs.Meetings
{
    public class NewMeeting
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public DateTime Start{ get; set; }
        public DateTime End { get; set; }
    }
}
