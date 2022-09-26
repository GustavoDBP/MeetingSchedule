using System;

namespace MeetingSchedule.Models.DTOs.Room
{
    public class GetRoomMeetings
    {
        public int RoomId { get; set; }
        public DateTime? InitialDateTime{ get; set; }
        public DateTime? FinalDateTime{ get; set; }
    }
}
