using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MeetingSchedule.Models.Entities
{
    public class Meeting
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        public ViewModels.Meeting ToViewModel()
        {
            return new ViewModels.Meeting()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Room = Room.ToViewModel(),
                Start = Start,
                End = End
            };
        }
    }
}
