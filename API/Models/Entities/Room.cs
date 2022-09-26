using MeetingSchedule.Models.ViewModels;
using System.Collections;
using System.Collections.Generic;

namespace MeetingSchedule.Models.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }

        public virtual ViewModels.Room ToViewModel()
        {
            return new ViewModels.Room()
            {
                Id = Id,
                Name = Name,
                Color = Color
            };
        }
    }
}
